mergeInto(LibraryManager.library, {

    ShowReadyPlayerMeFrame: function ()
    {
        var subdomain = '3dmeta';
        var rpmFrame = document.getElementById("rpm-frame");
        rpmFrame.src =`https://${subdomain != "" ? subdomain : "demo"}.readyplayer.me/avatar?frameApi`;

        var rpmContainer = document.getElementById("rpm-container");
        rpmContainer.style.display = "block";
    },

    Releaseiframe: function()
    {
        var rpmFrame = document.getElementById("rpm-frame");
        rpmFrame.src = '';
    },
  
    HideReadyPlayerMeFrame: function ()
    {
        var rpmContainer = document.getElementById("rpm-container");
        rpmContainer.style.display = "none";
        var rpmFrame = document.getElementById("rpm-frame");
        rpmFrame.src =``;
    },
        
    SetupRpm: function (partner)
    {
        setupRpmFrame(UTF8ToString(partner));
    },
});