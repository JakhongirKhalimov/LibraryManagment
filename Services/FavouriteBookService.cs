using AutoMapper;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Events;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels.FavouriteBookViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Services
{
    public class FavouriteBookService : IFavouriteBookService
    {
        #region Fields
        private readonly IBaseRepository<FavouriteBook> _favouriteBookRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Cosntructor
        public FavouriteBookService(IBaseRepository<FavouriteBook> favouriteBookRepository,
                                    IHttpContextAccessor contextAccessor,
                                    IBaseRepository<User> userRepository,
                                    IMapper mapper,
                                    IBaseRepository<Book> bookRepository,
                                    IEventPublisher eventPublisher)
        {
            _favouriteBookRepository = favouriteBookRepository;
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
            _bookRepository = bookRepository;
            _eventPublisher = eventPublisher;
        }
        #endregion

        #region Methods
        public async Task<BaseResponseModel> CreateAsync(CreateFavouriteBookViewModel createFavouriteBookViewModel)
        {
            if(createFavouriteBookViewModel != null)
            {
                FavouriteBook favouriteBook = new FavouriteBook
                {
                    UserId = createFavouriteBookViewModel.UserId,
                    BookId = createFavouriteBookViewModel.BookId,
                    IsFavourite = true
                };

                _favouriteBookRepository.Add(favouriteBook);

                bool isSuccessful = await _favouriteBookRepository.SaveChangesAsync();
                if (isSuccessful)
                {
                    var book = await _bookRepository.GetByCondition(_ => _.Id == createFavouriteBookViewModel.BookId).FirstOrDefaultAsync();
                    
                    if (book != null)
                    {
                        var favouriteBokToggleEvent = new FavouriteBookToggledEvent(book, favouriteBook);
                        await _eventPublisher.PublishAsync(favouriteBokToggleEvent);
                    }

                    return new BaseResponseModel
                    {
                        IsValid = true,
                    };
                }
                else
                {
                    return new BaseResponseModel
                    {
                        IsValid = false,
                    };
                }
            }
            return new BaseResponseModel
            {
                IsValid = false,
            };
        }

        [HttpGet]
        public async Task<BaseModel<FavouriteBookViewModel>> IsFavouriteAsync(int BookId)
        {
            var isFavouriteResponse = new BaseModel<FavouriteBookViewModel>();
            var currUserName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

            var currUser = await _userRepository.GetByCondition(_ => _.UserName == currUserName).FirstOrDefaultAsync();

            if (currUser != null)
            {
                var favouriteBook = await _favouriteBookRepository.GetByCondition(_ => _.BookId == BookId && _.UserId == currUser.Id).FirstOrDefaultAsync();

                if (favouriteBook != null)
                {
                    isFavouriteResponse.Value = _mapper.Map<FavouriteBookViewModel>(favouriteBook);
                }
                else
                {
                    isFavouriteResponse.Value = new FavouriteBookViewModel();
                }
                isFavouriteResponse.IsValid = true;
            }
            else
            {
                isFavouriteResponse.IsValid = false;
            }

            return isFavouriteResponse;
        }

        [HttpPost]
        public async Task<BaseResponseModel> ToggleFavouriteAsync(int bookId)
        {
            var response = new BaseResponseModel();

            var currUserName = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

            var currUser = await _userRepository.GetByCondition(_ => _.UserName == currUserName).FirstOrDefaultAsync();

            if (currUser != null)
            {
                var favouriteBook = await _favouriteBookRepository.GetByCondition(_ => _.BookId == bookId && _.UserId == currUser.Id).FirstOrDefaultAsync();

                if (favouriteBook != null)
                {
                    favouriteBook.IsFavourite = !favouriteBook.IsFavourite;

                    _favouriteBookRepository.Update(favouriteBook);

                    bool isSuccessful = await _favouriteBookRepository.SaveChangesAsync();

                    if (isSuccessful)
                    {
                        var book = await _bookRepository.GetByCondition(_ => _.Id == bookId).FirstOrDefaultAsync();

                        if (book != null)
                        {
                            var favouriteBokToggleEvent = new FavouriteBookToggledEvent(book, favouriteBook);
                            await _eventPublisher.PublishAsync(favouriteBokToggleEvent);
                        }
                        
                        response.IsValid = true;
                    }
                    else
                    {
                        response.IsValid = false;
                    }
                }
                else
                {
                    return await CreateAsync(new CreateFavouriteBookViewModel { UserId = currUser.Id, BookId = bookId});
                }
            }
            else
            {
                response.IsValid = false;
            }

            return response;
        }
        #endregion
    }
}
