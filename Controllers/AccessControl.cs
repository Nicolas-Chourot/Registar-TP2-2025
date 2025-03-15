﻿using JsonDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsonDemo.Controllers
{
    public class AccessControl
    {
        public class UserAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                User connectedUser = (User)HttpContext.Current.Session["ConnectedUser"];
                if (connectedUser == null)
                {
                    httpContext.Response.Redirect("/Accounts/Login?message=Accès non autorisé!&success=false");
                    return false;
                }
                else
                {
                    connectedUser = DB.Users.Get(connectedUser.Id);
                    if (connectedUser.Blocked)
                    {
                         return false;
                    }
                }
                return true;
            }
        }
        public class AdminAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                HttpContext.Current.Session["CRUD_Access"] = false;
                User connectedUser = (User)HttpContext.Current.Session["ConnectedUser"];
                if (connectedUser == null)
                {
                    httpContext.Response.Redirect("/Accounts/Login?message=Accès non autorisé!&success=false");
                    return false;
                }
                else
                {
                    connectedUser = DB.Users.Get(connectedUser.Id);
                    if (!connectedUser.IsAdmin)
                    {
                        if (connectedUser.Blocked)
                        {
                            return false;
                        }
                        else
                        {
                            httpContext.Response.Redirect("/Accounts/Login?message=Accès administrateur non autorisé!&success=false");
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

    }
}