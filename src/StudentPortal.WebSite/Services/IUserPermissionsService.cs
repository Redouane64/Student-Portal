using System;
using StudentPortal.WebSite.Models;

namespace StudentPortal.WebSite.Services
{
    public interface IUserPermissionsService
    {
        Boolean CanEditPost(Post post);

        Boolean CanEditPostComment(PostComment postComment);
    }
}