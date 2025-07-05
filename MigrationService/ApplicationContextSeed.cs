using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Receipts.Contracts;
using Receipts.DataBase;
using User = Authorization.DataBase.User;

namespace MigrationService;

// public class ApplicationContextSeed
// {
//     private readonly ICheckUseCase checkUseCase;
//     private readonly ReceiptsContext context;
//     private readonly IdentityUserContext<User, string> userContext;
//     private readonly IHostEnvironment hostEnvironment;
//     private readonly ILogger<ApplicationContextSeed> logger;
//     private readonly IReadOnlyList<SaveCheckRequest> requests;
//
//     public ApplicationContextSeed(
//         ICheckUseCase checkUseCase,
//         ReceiptsContext context,
//         IdentityUserContext<User, string> userContext,
//         ILogger<ApplicationContextSeed> logger,
//         IHostEnvironment hostEnvironment)
//     {
//         this.context = context;
//         this.userContext = userContext;
//         this.hostEnvironment = hostEnvironment;
//         this.checkUseCase = checkUseCase;
//         this.logger = logger;
//
//         requests = new[]
//             {
//                 "t=20250107T104157&s=1691.16&fn=7380440801290534&i=9446&fp=1880975916&n=1",
//                 "t=20250112T1715&s=93.00&fn=7381440800435707&i=21728&fp=3846028993&n=1",
//                 "t=20250113T2321&s=1015.73&fn=7284440500173838&i=62887&fp=182171056&n=1",
//                 "t=20250111T105907&s=1236.35&fn=7380440801290534&i=10277&fp=840967215&n=1",
//                 "t=20250104T1233&s=1289.00&fn=7380440700673345&i=26636&fp=1241320200&n=1"
//             }
//             .Select(raw => new CheckRequest(raw, DateTimeOffset.UtcNow))
//             .Select(t => new SaveCheckRequest
//             {
//                 Fd = t.Fd,
//                 S = t.S,
//                 T = t.T,
//                 Fn = t.Fn,
//                 Fp = t.Fp,
//                 Login = "00000000-0000-0000-0000-000000000000"
//             })
//             .ToList();
//     }
//
//     public async Task Seed(CancellationToken token = default)
//     {
//         try
//         {
//             await AddDefaultUser(token);
//             foreach (var request in requests) await checkUseCase.SaveCheck(request);
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Seeding error");
//             throw;
//         }
//     }
//
//     private Task AddDefaultUser(CancellationToken token = default)
//     {
//         if (userContext.Users.Any(user => user.Id == User.Default.Id)) return Task.CompletedTask;
//         
//         userContext.Users.Add(User.Default);
//         
//         return context.SaveChangesAsync(token);
//     }
// }