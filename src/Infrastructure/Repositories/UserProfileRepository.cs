// using AutoMapper;
// using Domain.Aggregates;
// using Domain.Interfaces;
// using Persistance.Data;
// using Shared.Model;
// using Shared.Results;
//
// namespace Infrastructure.Repositories;
//
// /// <summary>
// /// Repository for managing user profiles, including profile creation and status retrieval.
// /// </summary>
// public class UserProfileRepository
//     : RepositoryBase<UserProfileAggregate>, IUserProfileRepository
// {
//     private readonly IMapper _mapper;
//
//     /// <summary>
//     /// Initializes a new instance of the <see cref="UserProfileRepository"/> class.
//     /// </summary>
//     /// <param name="context">The application database context for accessing the database.</param>
//     /// <param name="mapper">The AutoMapper instance for mapping between models and entities.</param>
//     public UserProfileRepository(
//         ApplicationDbContext context,
//         IMapper mapper)
//
//         : base(context)
//     {
//         _mapper = mapper;
//
//     }
//     
//     /// <summary>
//     /// Creates a new user profile asynchronously.
//     /// </summary>
//     /// <param name="request">The user profile model containing the details of the profile to be created.</param>
//     /// <returns>A result containing the created user profile model or a failure message.</returns>
//     public async Task<Result<UserProfileModel>> CreateUserProfileAsync(UserProfileModel request)
//     {
//         var userProfileEntity = _mapper.Map<UserProfileAggregate>(request);
//         
//         var created = await CreateAsync(userProfileEntity);
//         
//         if(!created)
//             return Result<UserProfileModel>.Failure("Failed to create user account. Please try again.");
//     
//         var userProfileModel = _mapper.Map<UserProfileModel>(userProfileEntity);
//         
//         return Result<UserProfileModel>.Success(userProfileModel);
//     }
//     
//     /// <summary>
//     /// Retrieves the status of a user account asynchronously by user ID.
//     /// </summary>
//     /// <param name="userId">The user ID for which the account status is to be retrieved.</param>
//     /// <returns>The status of the user account.</returns>
//     public async Task<string> GetUserAccountStatusAsync(string userId)
//     {
//         var userAccount = await GetByIdAsync(userId);
//         return userAccount.Status;
//     }
// }