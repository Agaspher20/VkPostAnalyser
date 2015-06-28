﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services.Authentication;

namespace VkPostAnalyser.Services
{
    public interface ISocialApiProvider
    {
        Task<IList<PostInfo>> RetrievePostInfosAsync(int userId, ApplicationUser author);

        string BuildPostUrl(UserReport report, PostInfo post);
    }
}
