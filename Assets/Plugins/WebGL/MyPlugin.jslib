 var MyPlugin = {
     IsMobile: function()
     {
         isMobile = Module.SystemInfo.mobile;
         return Module.SystemInfo.mobile;
     },

    IsMobileModified: function()
    {
        var userAgent = navigator.userAgent;
        isMobile = (
                    /\b(BlackBerry|webOS|iPhone|IEMobile)\b/i.test(userAgent) ||
                    /\b(Android|Windows Phone|iPad|iPod)\b/i.test(userAgent) ||
                    // iPad on iOS 13 detection
                    (userAgent.includes("Mac") && "ontouchend" in document)
                );
        return isMobile;   
    },

    GetURLFromPage: function () {
        var returnStr = window.top.location.href;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    OpenNewTab : function(url)
    {
        url = Pointer_stringify(url);
        window.open(url,'_blank');
    },
 };
 
 mergeInto(LibraryManager.library, MyPlugin);