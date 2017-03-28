// site.js
(function () {
    var $sidebarAndWrapper = $("#sidebar,#wrapper"); //This is a wrapped set
    var $icon = $("#sidebarToggle i.fa");
    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");
        }
    });


})();



// This was the sample jquery - now commented out
//(function () {
  
//    var ele = $("#username");
//    ele.text("Patti Scalfano");
//    var main = $("#main");
//    main.on("mouseenter", function () {
//        // this doesn't work
//        main.style = "background-color: #888;";
//        // try this
//        main.css({ backgroundColor: "#888"});
//    });
//    main.on("mouseleave", function () {
//        // This doesn't work
//        main.style = "";
//        // This works
//        main.attr('style', '');
//    });

//    // this jquery returns multiple objects
//    var menuitems = $("ul.menu li a");
//    menuitems.on("click", function () {
//        var me = $(this);
//        alert("Hello " + me.text());
//    });

//    // This is the code with native javascript to do the same as above with jquery
//    //main.onmouseenter = function () {
//    //    main.style = "background: #888;";
//    //}
    
//    //main.onmouseleave = function () {
//    //    main.style.backgroundColor = "";
//    //}

//})();
