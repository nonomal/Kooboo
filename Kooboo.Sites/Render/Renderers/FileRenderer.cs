//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System.Threading.Tasks;
using Kooboo.Extensions;
using Kooboo.Sites.Extensions;
using Kooboo.Lib;
using Kooboo.Lib.Helper;
using System;
using Kooboo.Sites.Models;

namespace Kooboo.Sites.Render
{
    public class FileRenderer
    {
        private static long _bigsize;
        public static long BigSize
        {
            get
            {
                if (_bigsize == 0)
                {
                    _bigsize = 1024 * 1024 * 5;  // 5mb
                }
                return _bigsize;
            }
        }

        public static async Task RenderAsync(FrontContext context)
        {
            var file = context.SiteDb.Files.Get(context.Route.objectId, true);
            if (file == null)
            {
                return;
            }
            await RenderFile(context, file);
        }

        //file only contains column data. 
        public static async Task RenderFile(FrontContext context, Models.CmsFile file)
        {
            var sitedb = context.WebSite.SiteDb();
            string contentType = null;

            if (!string.IsNullOrEmpty(file.ContentType))
            {
                contentType = file.ContentType;
            }
            else if (!string.IsNullOrWhiteSpace(file.Name))
            {
                contentType = IOHelper.MimeType(file.Name);
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                if (context.Route != null)
                {
                    contentType = IOHelper.MimeType(context.Route.Name);
                }
            }

            context.RenderContext.Response.ContentType = contentType;

            if (file.Size > BigSize)
            {
                var partinfo = sitedb.Files.Store.GetFieldPart(file.Id, nameof(file.ContentBytes));

                try
                {
                    var range = context.RenderContext.Request.Headers.Get("Range");
                    if (range?.StartsWith("bytes=") ?? false)
                    {
                        partinfo.RangeStart = Convert.ToInt64(range.Substring(6).Split('-')[0]);

                        if (partinfo.RangeStart > 0)
                        {
                            context.RenderContext.Response.StatusCode = 206;
                        }
                    }
                }
                catch (Exception)
                {
                }

                context.RenderContext.Response.FilePart = partinfo;
            }
            else
            {
                // read whole content. 
                CmsFile FileContent = null;
                if (file.ContentBytes == null)
                {
                    FileContent = await sitedb.FilePool.GetAsync(file.Id);
                }
                else
                {
                    FileContent = file;
                }


                if (FileContent.ContentBytes != null)
                {
                    context.RenderContext.Response.Body = FileContent.ContentBytes;
                }
                else if (!string.IsNullOrEmpty(FileContent.ContentString))
                {
                    context.RenderContext.Response.Body = DataConstants.DefaultEncoding.GetBytes(FileContent.ContentString);
                }
            }

            // cache for font.
            if (contentType != null)
            {
                string lower = contentType.ToLower();

                if (lower.Contains("font"))
                {
                    VersionRenderer.FontVersion(context);
                }
                else if (lower.Contains("image"))
                {
                    context.RenderContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.RenderContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");

                    VersionRenderer.ImageVersion(context);
                }
                else if (lower.Contains("css"))
                {
                    VersionRenderer.ScriptStyleVersion(context);
                }
                else if (lower.Contains("javascript"))
                {
                    VersionRenderer.ScriptStyleVersion(context);
                }
                else if (lower.Contains("video"))
                {
                    VersionRenderer.VideoVersion(context);
                }
            }

        }
    }
}
