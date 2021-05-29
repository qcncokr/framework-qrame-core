using System;
using System.IO;
using System.Collections.Generic;

namespace Qrame.CoreFX
{
    /// <summary>
    /// 웹 응용 프로그램에서 사용 할 수 있는 파일 확장자에 따라 Mime 값을 관리하는 컬렉션입니다.
    /// </summary>
    public static class MimeMapping
    {
        private static readonly IDictionary<string, string> mimeDictionary;
        private static readonly IDictionary<string, string> microsoftMimeDictionary;

        /// <summary>
        /// 웹 응용 프로그램에서 사용 할 수 있는 파일 확장자에 따라 Mime 값을 구성합니다.
        /// </summary>
        static MimeMapping()
        {
            Dictionary<string, string> mimes = new Dictionary<string, string>();
            mimes.Add(".*", "application/octet-stream");
            mimes.Add(".323", "text/h323");
            mimes.Add(".aaf", "application/octet-stream");
            mimes.Add(".aca", "application/octet-stream");
            mimes.Add(".accdb", "application/msaccess");
            mimes.Add(".accde", "application/msaccess");
            mimes.Add(".accdt", "application/msaccess");
            mimes.Add(".acx", "application/internet-property-stream");
            mimes.Add(".afm", "application/octet-stream");
            mimes.Add(".ai", "application/postscript");
            mimes.Add(".aif", "audio/x-aiff");
            mimes.Add(".aifc", "audio/aiff");
            mimes.Add(".aiff", "audio/aiff");
            mimes.Add(".application", "application/x-ms-application");
            mimes.Add(".art", "image/x-jg");
            mimes.Add(".asd", "application/octet-stream");
            mimes.Add(".asf", "video/x-ms-asf");
            mimes.Add(".asi", "application/octet-stream");
            mimes.Add(".asm", "text/plain");
            mimes.Add(".asr", "video/x-ms-asf");
            mimes.Add(".asx", "video/x-ms-asf");
            mimes.Add(".atom", "application/atom+xml");
            mimes.Add(".au", "audio/basic");
            mimes.Add(".avi", "video/x-msvideo");
            mimes.Add(".axs", "application/olescript");
            mimes.Add(".bas", "text/plain");
            mimes.Add(".bcpio", "application/x-bcpio");
            mimes.Add(".bin", "application/octet-stream");
            mimes.Add(".bmp", "image/bmp");
            mimes.Add(".c", "text/plain");
            mimes.Add(".cab", "application/octet-stream");
            mimes.Add(".calx", "application/vnd.ms-office.calx");
            mimes.Add(".cat", "application/vnd.ms-pki.seccat");
            mimes.Add(".cdf", "application/x-cdf");
            mimes.Add(".chm", "application/octet-stream");
            mimes.Add(".class", "application/x-java-applet");
            mimes.Add(".clp", "application/x-msclip");
            mimes.Add(".cmx", "image/x-cmx");
            mimes.Add(".cnf", "text/plain");
            mimes.Add(".cod", "image/cis-cod");
            mimes.Add(".cpio", "application/x-cpio");
            mimes.Add(".cpp", "text/plain");
            mimes.Add(".crd", "application/x-mscardfile");
            mimes.Add(".crl", "application/pkix-crl");
            mimes.Add(".crt", "application/x-x509-ca-cert");
            mimes.Add(".csh", "application/x-csh");
            mimes.Add(".css", "text/css");
            mimes.Add(".csv", "application/octet-stream");
            mimes.Add(".cur", "application/octet-stream");
            mimes.Add(".dcr", "application/x-director");
            mimes.Add(".deploy", "application/octet-stream");
            mimes.Add(".der", "application/x-x509-ca-cert");
            mimes.Add(".dib", "image/bmp");
            mimes.Add(".dir", "application/x-director");
            mimes.Add(".disco", "text/xml");
            mimes.Add(".dll", "application/x-msdownload");
            mimes.Add(".dll.config", "text/xml");
            mimes.Add(".dlm", "text/dlm");
            mimes.Add(".doc", "application/msword");
            mimes.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
            mimes.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            mimes.Add(".dot", "application/msword");
            mimes.Add(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
            mimes.Add(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            mimes.Add(".dsp", "application/octet-stream");
            mimes.Add(".dtd", "text/xml");
            mimes.Add(".dvi", "application/x-dvi");
            mimes.Add(".dwf", "drawing/x-dwf");
            mimes.Add(".dwp", "application/octet-stream");
            mimes.Add(".dxr", "application/x-director");
            mimes.Add(".eml", "message/rfc822");
            mimes.Add(".emz", "application/octet-stream");
            mimes.Add(".eot", "application/octet-stream");
            mimes.Add(".eps", "application/postscript");
            mimes.Add(".etx", "text/x-setext");
            mimes.Add(".evy", "application/envoy");
            mimes.Add(".exe", "application/octet-stream");
            mimes.Add(".exe.config", "text/xml");
            mimes.Add(".fdf", "application/vnd.fdf");
            mimes.Add(".fif", "application/fractals");
            mimes.Add(".fla", "application/octet-stream");
            mimes.Add(".flr", "x-world/x-vrml");
            mimes.Add(".flv", "video/x-flv");
            mimes.Add(".gif", "image/gif");
            mimes.Add(".gtar", "application/x-gtar");
            mimes.Add(".gz", "application/x-gzip");
            mimes.Add(".h", "text/plain");
            mimes.Add(".hdf", "application/x-hdf");
            mimes.Add(".hdml", "text/x-hdml");
            mimes.Add(".hhc", "application/x-oleobject");
            mimes.Add(".hhk", "application/octet-stream");
            mimes.Add(".hhp", "application/octet-stream");
            mimes.Add(".hlp", "application/winhlp");
            mimes.Add(".hqx", "application/mac-binhex40");
            mimes.Add(".hta", "application/hta");
            mimes.Add(".htc", "text/x-component");
            mimes.Add(".htm", "text/html");
            mimes.Add(".html", "text/html");
            mimes.Add(".htt", "text/webviewhtml");
            mimes.Add(".hxt", "text/html");
            mimes.Add(".ico", "image/x-icon");
            mimes.Add(".ics", "application/octet-stream");
            mimes.Add(".ief", "image/ief");
            mimes.Add(".iii", "application/x-iphone");
            mimes.Add(".inf", "application/octet-stream");
            mimes.Add(".ins", "application/x-internet-signup");
            mimes.Add(".isp", "application/x-internet-signup");
            mimes.Add(".IVF", "video/x-ivf");
            mimes.Add(".jar", "application/java-archive");
            mimes.Add(".java", "application/octet-stream");
            mimes.Add(".jck", "application/liquidmotion");
            mimes.Add(".jcz", "application/liquidmotion");
            mimes.Add(".jfif", "image/pjpeg");
            mimes.Add(".jpb", "application/octet-stream");
            mimes.Add(".jpe", "image/jpeg");
            mimes.Add(".jpeg", "image/jpeg");
            mimes.Add(".jpg", "image/jpeg");
            mimes.Add(".js", "application/x-javascript");
            mimes.Add(".jsx", "text/jscript");
            mimes.Add(".latex", "application/x-latex");
            mimes.Add(".lit", "application/x-ms-reader");
            mimes.Add(".lpk", "application/octet-stream");
            mimes.Add(".lsf", "video/x-la-asf");
            mimes.Add(".lsx", "video/x-la-asf");
            mimes.Add(".lzh", "application/octet-stream");
            mimes.Add(".m13", "application/x-msmediaview");
            mimes.Add(".m14", "application/x-msmediaview");
            mimes.Add(".m1v", "video/mpeg");
            mimes.Add(".m3u", "audio/x-mpegurl");
            mimes.Add(".man", "application/x-troff-man");
            mimes.Add(".manifest", "application/x-ms-manifest");
            mimes.Add(".map", "text/plain");
            mimes.Add(".mdb", "application/x-msaccess");
            mimes.Add(".mdp", "application/octet-stream");
            mimes.Add(".me", "application/x-troff-me");
            mimes.Add(".mht", "message/rfc822");
            mimes.Add(".mhtml", "message/rfc822");
            mimes.Add(".mid", "audio/mid");
            mimes.Add(".midi", "audio/mid");
            mimes.Add(".mix", "application/octet-stream");
            mimes.Add(".mmf", "application/x-smaf");
            mimes.Add(".mno", "text/xml");
            mimes.Add(".mny", "application/x-msmoney");
            mimes.Add(".mov", "video/quicktime");
            mimes.Add(".movie", "video/x-sgi-movie");
            mimes.Add(".mp2", "video/mpeg");
            mimes.Add(".mp3", "audio/mpeg");
            mimes.Add(".mpa", "video/mpeg");
            mimes.Add(".mpe", "video/mpeg");
            mimes.Add(".mpeg", "video/mpeg");
            mimes.Add(".mpg", "video/mpeg");
            mimes.Add(".mpp", "application/vnd.ms-project");
            mimes.Add(".mpv2", "video/mpeg");
            mimes.Add(".ms", "application/x-troff-ms");
            mimes.Add(".msi", "application/octet-stream");
            mimes.Add(".mso", "application/octet-stream");
            mimes.Add(".mvb", "application/x-msmediaview");
            mimes.Add(".mvc", "application/x-miva-compiled");
            mimes.Add(".nc", "application/x-netcdf");
            mimes.Add(".nsc", "video/x-ms-asf");
            mimes.Add(".nws", "message/rfc822");
            mimes.Add(".ocx", "application/octet-stream");
            mimes.Add(".oda", "application/oda");
            mimes.Add(".odc", "text/x-ms-odc");
            mimes.Add(".ods", "application/oleobject");
            mimes.Add(".one", "application/onenote");
            mimes.Add(".onea", "application/onenote");
            mimes.Add(".onetoc", "application/onenote");
            mimes.Add(".onetoc2", "application/onenote");
            mimes.Add(".onetmp", "application/onenote");
            mimes.Add(".onepkg", "application/onenote");
            mimes.Add(".osdx", "application/opensearchdescription+xml");
            mimes.Add(".p10", "application/pkcs10");
            mimes.Add(".p12", "application/x-pkcs12");
            mimes.Add(".p7b", "application/x-pkcs7-certificates");
            mimes.Add(".p7c", "application/pkcs7-mime");
            mimes.Add(".p7m", "application/pkcs7-mime");
            mimes.Add(".p7r", "application/x-pkcs7-certreqresp");
            mimes.Add(".p7s", "application/pkcs7-signature");
            mimes.Add(".pbm", "image/x-portable-bitmap");
            mimes.Add(".pcx", "application/octet-stream");
            mimes.Add(".pcz", "application/octet-stream");
            mimes.Add(".pdf", "application/pdf");
            mimes.Add(".pfb", "application/octet-stream");
            mimes.Add(".pfm", "application/octet-stream");
            mimes.Add(".pfx", "application/x-pkcs12");
            mimes.Add(".pgm", "image/x-portable-graymap");
            mimes.Add(".pko", "application/vnd.ms-pki.pko");
            mimes.Add(".pma", "application/x-perfmon");
            mimes.Add(".pmc", "application/x-perfmon");
            mimes.Add(".pml", "application/x-perfmon");
            mimes.Add(".pmr", "application/x-perfmon");
            mimes.Add(".pmw", "application/x-perfmon");
            mimes.Add(".png", "image/png");
            mimes.Add(".pnm", "image/x-portable-anymap");
            mimes.Add(".pnz", "image/png");
            mimes.Add(".pot", "application/vnd.ms-powerpoint");
            mimes.Add(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
            mimes.Add(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
            mimes.Add(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
            mimes.Add(".ppm", "image/x-portable-pixmap");
            mimes.Add(".pps", "application/vnd.ms-powerpoint");
            mimes.Add(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            mimes.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            mimes.Add(".ppt", "application/vnd.ms-powerpoint");
            mimes.Add(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            mimes.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            mimes.Add(".prf", "application/pics-rules");
            mimes.Add(".prm", "application/octet-stream");
            mimes.Add(".prx", "application/octet-stream");
            mimes.Add(".ps", "application/postscript");
            mimes.Add(".psd", "application/octet-stream");
            mimes.Add(".psm", "application/octet-stream");
            mimes.Add(".psp", "application/octet-stream");
            mimes.Add(".pub", "application/x-mspublisher");
            mimes.Add(".qt", "video/quicktime");
            mimes.Add(".qtl", "application/x-quicktimeplayer");
            mimes.Add(".qxd", "application/octet-stream");
            mimes.Add(".ra", "audio/x-pn-realaudio");
            mimes.Add(".ram", "audio/x-pn-realaudio");
            mimes.Add(".rar", "application/octet-stream");
            mimes.Add(".ras", "image/x-cmu-raster");
            mimes.Add(".rf", "image/vnd.rn-realflash");
            mimes.Add(".rgb", "image/x-rgb");
            mimes.Add(".rm", "application/vnd.rn-realmedia");
            mimes.Add(".rmi", "audio/mid");
            mimes.Add(".roff", "application/x-troff");
            mimes.Add(".rpm", "audio/x-pn-realaudio-plugin");
            mimes.Add(".rtf", "application/rtf");
            mimes.Add(".rtx", "text/richtext");
            mimes.Add(".scd", "application/x-msschedule");
            mimes.Add(".sct", "text/scriptlet");
            mimes.Add(".sea", "application/octet-stream");
            mimes.Add(".setpay", "application/set-payment-initiation");
            mimes.Add(".setreg", "application/set-registration-initiation");
            mimes.Add(".sgml", "text/sgml");
            mimes.Add(".sh", "application/x-sh");
            mimes.Add(".shar", "application/x-shar");
            mimes.Add(".sit", "application/x-stuffit");
            mimes.Add(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12");
            mimes.Add(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
            mimes.Add(".smd", "audio/x-smd");
            mimes.Add(".smi", "application/octet-stream");
            mimes.Add(".smx", "audio/x-smd");
            mimes.Add(".smz", "audio/x-smd");
            mimes.Add(".snd", "audio/basic");
            mimes.Add(".snp", "application/octet-stream");
            mimes.Add(".spc", "application/x-pkcs7-certificates");
            mimes.Add(".spl", "application/futuresplash");
            mimes.Add(".src", "application/x-wais-source");
            mimes.Add(".ssm", "application/streamingmedia");
            mimes.Add(".sst", "application/vnd.ms-pki.certstore");
            mimes.Add(".stl", "application/vnd.ms-pki.stl");
            mimes.Add(".sv4cpio", "application/x-sv4cpio");
            mimes.Add(".sv4crc", "application/x-sv4crc");
            mimes.Add(".swf", "application/x-shockwave-flash");
            mimes.Add(".t", "application/x-troff");
            mimes.Add(".tar", "application/x-tar");
            mimes.Add(".tcl", "application/x-tcl");
            mimes.Add(".tex", "application/x-tex");
            mimes.Add(".texi", "application/x-texinfo");
            mimes.Add(".texinfo", "application/x-texinfo");
            mimes.Add(".tgz", "application/x-compressed");
            mimes.Add(".thmx", "application/vnd.ms-officetheme");
            mimes.Add(".thn", "application/octet-stream");
            mimes.Add(".tif", "image/tiff");
            mimes.Add(".tiff", "image/tiff");
            mimes.Add(".toc", "application/octet-stream");
            mimes.Add(".tr", "application/x-troff");
            mimes.Add(".trm", "application/x-msterminal");
            mimes.Add(".tsv", "text/tab-separated-values");
            mimes.Add(".ttf", "application/octet-stream");
            mimes.Add(".txt", "text/plain");
            mimes.Add(".u32", "application/octet-stream");
            mimes.Add(".uls", "text/iuls");
            mimes.Add(".ustar", "application/x-ustar");
            mimes.Add(".vbs", "text/vbscript");
            mimes.Add(".vcf", "text/x-vcard");
            mimes.Add(".vcs", "text/plain");
            mimes.Add(".vdx", "application/vnd.ms-visio.viewer");
            mimes.Add(".vml", "text/xml");
            mimes.Add(".vsd", "application/vnd.visio");
            mimes.Add(".vss", "application/vnd.visio");
            mimes.Add(".vst", "application/vnd.visio");
            mimes.Add(".vsto", "application/x-ms-vsto");
            mimes.Add(".vsw", "application/vnd.visio");
            mimes.Add(".vsx", "application/vnd.visio");
            mimes.Add(".vtx", "application/vnd.visio");
            mimes.Add(".wav", "audio/wav");
            mimes.Add(".wax", "audio/x-ms-wax");
            mimes.Add(".wbmp", "image/vnd.wap.wbmp");
            mimes.Add(".wcm", "application/vnd.ms-works");
            mimes.Add(".wdb", "application/vnd.ms-works");
            mimes.Add(".wks", "application/vnd.ms-works");
            mimes.Add(".wm", "video/x-ms-wm");
            mimes.Add(".wma", "audio/x-ms-wma");
            mimes.Add(".wmd", "application/x-ms-wmd");
            mimes.Add(".wmf", "application/x-msmetafile");
            mimes.Add(".wml", "text/vnd.wap.wml");
            mimes.Add(".wmlc", "application/vnd.wap.wmlc");
            mimes.Add(".wmls", "text/vnd.wap.wmlscript");
            mimes.Add(".wmlsc", "application/vnd.wap.wmlscriptc");
            mimes.Add(".wmp", "video/x-ms-wmp");
            mimes.Add(".wmv", "video/x-ms-wmv");
            mimes.Add(".wmx", "video/x-ms-wmx");
            mimes.Add(".wmz", "application/x-ms-wmz");
            mimes.Add(".wps", "application/vnd.ms-works");
            mimes.Add(".wri", "application/x-mswrite");
            mimes.Add(".wrl", "x-world/x-vrml");
            mimes.Add(".wrz", "x-world/x-vrml");
            mimes.Add(".wsdl", "text/xml");
            mimes.Add(".wvx", "video/x-ms-wvx");
            mimes.Add(".x", "application/directx");
            mimes.Add(".xaf", "x-world/x-vrml");
            mimes.Add(".xaml", "application/xaml+xml");
            mimes.Add(".xap", "application/x-silverlight-app");
            mimes.Add(".xbap", "application/x-ms-xbap");
            mimes.Add(".xbm", "image/x-xbitmap");
            mimes.Add(".xdr", "text/plain");
            mimes.Add(".xla", "application/vnd.ms-excel");
            mimes.Add(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
            mimes.Add(".xlc", "application/vnd.ms-excel");
            mimes.Add(".xlm", "application/vnd.ms-excel");
            mimes.Add(".xls", "application/vnd.ms-excel");
            mimes.Add(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            mimes.Add(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
            mimes.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            mimes.Add(".xlt", "application/vnd.ms-excel");
            mimes.Add(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
            mimes.Add(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            mimes.Add(".xlw", "application/vnd.ms-excel");
            mimes.Add(".xml", "text/xml");
            mimes.Add(".xof", "x-world/x-vrml");
            mimes.Add(".xpm", "image/x-xpixmap");
            mimes.Add(".xps", "application/vnd.ms-xpsdocument");
            mimes.Add(".xsd", "text/xml");
            mimes.Add(".xsf", "text/xml");
            mimes.Add(".xsl", "text/xml");
            mimes.Add(".xslt", "text/xml");
            mimes.Add(".xsn", "application/octet-stream");
            mimes.Add(".xtp", "application/octet-stream");
            mimes.Add(".xwd", "image/x-xwindowdump");
            mimes.Add(".z", "application/x-isCompress");
            mimes.Add(".zip", "application/x-zip-compressed");

            Dictionary<string, string> microsoftMimes = new Dictionary<string, string>();

            microsoftMimes.Add(".xap", "application/x-silverlight-app");
            microsoftMimes.Add(".manifest", "application/manifest");
            microsoftMimes.Add(".application", "application/x-ms-application");
            microsoftMimes.Add(".xbap", "application/x-ms-xbap");
            microsoftMimes.Add(".deploy", "application/octet-stream");
            microsoftMimes.Add(".xps", "application/vnd.ms-xpsdocument");
            microsoftMimes.Add(".xaml", "application/xaml+xml");
            microsoftMimes.Add(".cab", "application/vnd.ms-cab-compressed");
            microsoftMimes.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            microsoftMimes.Add(".docm", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            microsoftMimes.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            microsoftMimes.Add(".pptm", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            microsoftMimes.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            microsoftMimes.Add(".xlsm", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            microsoftMimes.Add(".accdb", "application/msaccess");
            microsoftMimes.Add(".pub", "application/x-mspublisher");
            microsoftMimes.Add(".svg", "image/svg+xml");
            microsoftMimes.Add(".xht", "application/xhtml+xml");
            microsoftMimes.Add(".xhtml", "application/xhtml+xml");

            mimeDictionary = mimes;
            microsoftMimeDictionary = microsoftMimes;
        }

        /// <summary>
        /// 웹 응용 프로그램에서 사용 할 수 있는 파일 확장자에 따라 Mime 값을 추가합니다.
        /// </summary>
        /// <param name="extension">Mime Mapping 컬렉션의 키 값을 가질 파일 확장자입니다.</param>
        /// <param name="mimeType">파일 확장자에 따라 Mime 값을 반환할 값입니다.</param>
        public static void AddMimeMapping(string extension, string mimeType)
        {
            mimeDictionary.Add(extension, mimeType);
        }

        /// <summary>
        /// 웹 응용 프로그램에서 사용 할 수 있는 파일 확장자에 따라 Mime 값을 반환합니다.
        /// </summary>
        /// <param name="fileName">Mime 값을 가져올 파일명입니다. 확장자에 맞는 Mime이 없을 경우 application/octet-stream를 기본값으로 반환합니다.</param>
        /// <returns>파일 확장자에 맞는 Mime 값입니다.</returns>
        public static string GetMimeMapping(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) == true)
            {
                throw new ArgumentNullException("Mime 값을 가져올 파일명을 입력해야합니다.");
            }

            string mime = null;
            if (mimeDictionary.TryGetValue(Path.GetExtension(fileName), out mime) == true)
            {
                return mime;
            }

            return mimeDictionary[".*"];
        }
    }
}
