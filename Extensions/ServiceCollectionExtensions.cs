using LibraryManagementSystem.Events;
using LibraryManagementSystem.Infastructure;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationSettings(this IServiceCollection services)
        {
            CommonHelper.DefaultFileProvider = new AppFileProvider();
            Singleton<ITypeFinder>.Instance = new TypeFinder();

            services.AddHttpContextAccessor();

            services.AddSingleton(Singleton<ITypeFinder>.Instance);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IReflectionRepository, ReflectionRepository>();

            services.AddSingleton<IImageFileService, ImageFileService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IBookCategoryService, BookCategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ITranslatableService, TranslatableService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IFavouriteBookService, FavouriteBookService>();
            services.AddScoped<INotificationService, NotificationService>();

            var consumers = Singleton<ITypeFinder>.Instance.FindClassesOfType(typeof(IConsumer<>)).ToList();

            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);
        }
    }
}