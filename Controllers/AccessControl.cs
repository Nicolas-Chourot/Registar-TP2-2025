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
                try
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
                        if (connectedUser.Blocked || !connectedUser.IsOnline)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public class AdminAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                try
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
                        if (!connectedUser.IsAdmin)
                        {
                            if (connectedUser.Blocked || !connectedUser.IsOnline)
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
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}