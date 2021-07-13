var FollowController = function () {
    var btn;
    var init = () => {
        initFollow();
        $(".js-toggle-follow").click(toggleFollow);
    };
    var toggleFollow = e => {
        btn = $(e.target);
        if (btn.hasClass("btn-default")) follow();
        else unfollow();
    }
    var follow = () => {
        $.post("/api/followings", { followeeId: btn.attr("data-user-id") })
            .done(done)
            .fail(fail);
    }
    var unfollow = () => {
        $.ajax({
            url: "/api/followings/" + btn.attr("data-user-id"),
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    }
    var initFollow = () => {
        $.get("/api/followings")
            .done(data => {
                $(".js-toggle-follow").each((i, o) => {
                    if (data.filter(a => a.FolloweeId === o.getAttribute("data-user-id")).length !== 0) {
                        o.innerText = "Following";
                        o.classList.remove("btn-default");
                        o.classList.add("btn-info");
                    }
                })
            })
    }
    var done = () => {
        var text = (btn.text() == "Following") ? "Follow" : "Following";
        btn.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }
    var fail = (e) => {
        bootbox.alert(typeof e.responseJSON.Message != "undefined" ? e.responseJSON.Message : "Something failed!");
    }
    return {
        init
    }
}();