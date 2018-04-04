$(document).ready(function () {
        $(".sub > a").click(function () {
            var ul = $(this).next(),
                clone = ul.clone().css({
                    "height": "auto"
                }).appendTo(".mini-menu"),
                height = ul.css("height") === "0px" ? ul[0].scrollHeight + "px" : "0px";
            clone.remove();
            ul.animate({
                "height": height
            });
            return false;
        });
    });