using MassTransit;
using NotificationService.Consumers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var rabbitMqHost = context.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";

        services.AddMassTransit(x =>
        {
            // Register consumer
            x.AddConsumer<BookingConfirmedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitMqHost, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Configure retry policy: 3 retries with exponential backoff
                cfg.ReceiveEndpoint("booking-confirmed-queue", e =>
                {
                    // Retry up to 3 times with exponential intervals (2s, 4s, 8s)
                    e.UseMessageRetry(r => r.Exponential(3,
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(30),
                        TimeSpan.FromSeconds(2)));

                    // Move to dead-letter queue after all retries fail
                    e.ConfigureConsumer<BookingConfirmedConsumer>(ctx);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });
    })
    .Build();

await host.RunAsync();
