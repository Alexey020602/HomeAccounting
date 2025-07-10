using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Utils;

/// <summary>
/// Предоставляет методы расширения для <see cref="IServiceCollection"/> для регистрации одной реализации как нескольких сервисов с общим временем жизни.
/// </summary>
public static class ServiceCollectionMultipleRegistration
{
    /// <summary>
    /// Регистрирует реализацию через фабрику как несколько сервисов с указанным временем жизни.
    /// </summary>
    /// <typeparam name="TService">Тип реализации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Время жизни сервиса (Singleton, Transient, Scoped).</param>
    /// <param name="implementationFactory">Фабрика для создания экземпляра реализации.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <typeparamref name="TService"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="serviceTypes"/> или <paramref name="implementationFactory"/> равны null.</exception>
    public static IServiceCollection AddAsMultipleServices<TService>(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Func<IServiceProvider, TService> implementationFactory,
        params Type[] serviceTypes
    )
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceTypes);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        services.Add(new ServiceDescriptor(typeof(TService), implementationFactory, lifetime));

        return AddServicesForService(services, typeof(TService), serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию по типу как несколько сервисов с указанным временем жизни.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="lifetime">Время жизни сервиса (Singleton, Transient, Scoped).</param>
    /// <param name="implementationType">Тип реализации сервиса.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <paramref name="implementationType"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationType"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddAsMultipleServices(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType,
        params Type[] serviceTypes
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(implementationType);
        ArgumentNullException.ThrowIfNull(serviceTypes);

        services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));

        return AddServicesForService(services, implementationType, serviceTypes);
    }

    #region Sigleton Methods

    /// <summary>
    /// Регистрирует реализацию через фабрику как несколько сервисов с временем жизни Singleton.
    /// </summary>
    /// <typeparam name="TService">Тип реализации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationFactory">Фабрика для создания экземпляра реализации.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <typeparamref name="TService"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationFactory"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddSingletonAsMultipleServices<TService>(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Func<IServiceProvider, TService> implementationFactory,
        params Type[] serviceTypes
    )
        where TService : class
    {
        return services.AddAsMultipleServices(ServiceLifetime.Singleton, implementationFactory, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию по типу как несколько сервисов с временем жизни Singleton.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationType">Тип реализации сервиса.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <paramref name="implementationType"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationType"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddSingletonAsMultipleServices(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType,
        params Type[] serviceTypes
    )
    {
        return services.AddAsMultipleServices(ServiceLifetime.Singleton, implementationType, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию как два сервиса с временем жизни Singleton.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/> и <typeparamref name="TServiceSecond"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddSingletonAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TImplementation : class, TServiceFirst, TServiceSecond
    {
        return services.AddSingletonAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond)
        );
    }

    /// <summary>
    /// Регистрирует реализацию как три сервиса с временем жизни Singleton.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TServiceThird">Третий тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/>, <typeparamref name="TServiceSecond"/> и <typeparamref name="TServiceThird"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddSingletonAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        TServiceThird,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TServiceThird : class
        where TImplementation : class, TServiceFirst, TServiceSecond, TServiceThird
    {
        return services.AddSingletonAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond),
            typeof(TServiceThird)
        );
    }

    #endregion Singleton methods

    #region Transient methods

    /// <summary>
    /// Регистрирует реализацию через фабрику как несколько сервисов с временем жизни Transient.
    /// </summary>
    /// <typeparam name="TService">Тип реализации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationFactory">Фабрика для создания экземпляра реализации.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <typeparamref name="TService"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationFactory"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddTransientAsMultipleServices<TService>(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Func<IServiceProvider, TService> implementationFactory,
        params Type[] serviceTypes
    )
        where TService : class
    {
        return services.AddAsMultipleServices(ServiceLifetime.Transient, implementationFactory, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию по типу как несколько сервисов с временем жизни Transient.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationType">Тип реализации сервиса.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <paramref name="implementationType"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationType"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddTransientAsMultipleServices(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType,
        params Type[] serviceTypes
    )
    {
        return services.AddAsMultipleServices(ServiceLifetime.Transient, implementationType, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию как два сервиса с временем жизни Transient.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/> и <typeparamref name="TServiceSecond"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddTransientAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TImplementation : class, TServiceFirst, TServiceSecond
    {
        return services.AddTransientAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond)
        );
    }

    /// <summary>
    /// Регистрирует реализацию как три сервиса с временем жизни Transient.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TServiceThird">Третий тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/>, <typeparamref name="TServiceSecond"/> и <typeparamref name="TServiceThird"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddTransientAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        TServiceThird,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TServiceThird : class
        where TImplementation : class, TServiceFirst, TServiceSecond, TServiceThird
    {
        return services.AddTransientAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond),
            typeof(TServiceThird)
        );
    }

    #endregion Transient methods

    #region Scoped methods

    /// <summary>
    /// Регистрирует реализацию через фабрику как несколько сервисов с временем жизни Scoped.
    /// </summary>
    /// <typeparam name="TService">Тип реализации сервиса.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationFactory">Фабрика для создания экземпляра реализации.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <typeparamref name="TService"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationFactory"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddScopedAsMultipleServices<TService>(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Func<IServiceProvider, TService> implementationFactory,
        params Type[] serviceTypes
    )
        where TService : class
    {
        return services.AddAsMultipleServices(ServiceLifetime.Scoped, implementationFactory, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию по типу как несколько сервисов с временем жизни Scoped.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationType">Тип реализации сервиса.</param>
    /// <param name="serviceTypes">Типы сервисов, которые реализует <paramref name="implementationType"/>.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationType"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddScopedAsMultipleServices(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType,
        params Type[] serviceTypes
    )
    {
        return services.AddAsMultipleServices(ServiceLifetime.Scoped, implementationType, serviceTypes);
    }

    /// <summary>
    /// Регистрирует реализацию как два сервиса с временем жизни Scoped.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/> и <typeparamref name="TServiceSecond"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddScopedAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TImplementation : class, TServiceFirst, TServiceSecond
    {
        return services.AddScopedAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond)
        );
    }

    /// <summary>
    /// Регистрирует реализацию как три сервиса с временем жизни Scoped.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TServiceThird">Третий тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/>, <typeparamref name="TServiceSecond"/> и <typeparamref name="TServiceThird"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddScopedAsMultipleServices<
        TServiceFirst,
        TServiceSecond,
        TServiceThird,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TImplementation
    >(this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TServiceThird : class
        where TImplementation : class, TServiceFirst, TServiceSecond, TServiceThird
    {
        return services.AddScopedAsMultipleServices(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond),
            typeof(TServiceThird)
        );
    }

    #endregion Scoped methods

    #region Add Services For Service Methods

    /// <summary>
    /// Добавляет сервис, который ссылается на реализацию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="serviceType">Тип сервиса.</param>
    /// <param name="implementationType">Тип реализации.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="serviceType"/> или <paramref name="implementationType"/> равны null.</exception>
    public static IServiceCollection AddServiceForService(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(serviceType);
        ArgumentNullException.ThrowIfNull(implementationType);
        return services.AddTransient(
            serviceType,
            sp => sp.GetRequiredService(implementationType)
        );
    }

    /// <summary>
    /// Добавляет сервис, который ссылается на указанную реализацию.
    /// </summary>
    /// <typeparam name="TService">Тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TService"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddServiceForService<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.AddServiceForService(typeof(TService), typeof(TImplementation));
    }

    /// <summary>
    /// Добавляет несколько сервисов, которые ссылаются на одну реализацию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="implementationType">Тип реализации.</param>
    /// <param name="serviceTypes">Типы сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/>, <paramref name="implementationType"/> или <paramref name="serviceTypes"/> равны null.</exception>
    public static IServiceCollection AddServicesForService(
        this IServiceCollection services,
        Type implementationType,
        params Type[] serviceTypes)
    {
        foreach (var serviceType in serviceTypes)
        {
            services.AddServiceForService(serviceType, implementationType);
        }

        return services;
    }

    /// <summary>
    /// Добавляет два сервиса, которые ссылаются на одну реализацию.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/> и <typeparamref name="TServiceSecond"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddServicesForService<TServiceFirst, TServiceSecond, TImplementation>(
        this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TImplementation : class, TServiceFirst, TServiceSecond
    {
        return services.AddServicesForService(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond)
        );
    }

    /// <summary>
    /// Добавляет три сервиса, которые ссылаются на одну реализацию.
    /// </summary>
    /// <typeparam name="TServiceFirst">Первый тип сервиса.</typeparam>
    /// <typeparam name="TServiceSecond">Второй тип сервиса.</typeparam>
    /// <typeparam name="TServiceThird">Третий тип сервиса.</typeparam>
    /// <typeparam name="TImplementation">Тип реализации, который реализует <typeparamref name="TServiceFirst"/>, <typeparamref name="TServiceSecond"/> и <typeparamref name="TServiceThird"/>.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Обновленная коллекция сервисов.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="services"/> равно null.</exception>
    public static IServiceCollection AddServicesForService<TServiceFirst, TServiceSecond, TServiceThird,
        TImplementation>(
        this IServiceCollection services)
        where TServiceFirst : class
        where TServiceSecond : class
        where TServiceThird : class
        where TImplementation : class, TServiceFirst, TServiceSecond, TServiceThird
    {
        return services.AddServicesForService(
            typeof(TImplementation),
            typeof(TServiceFirst),
            typeof(TServiceSecond),
            typeof(TServiceThird)
        );
    }

    #endregion Add Services For Service Methods
}