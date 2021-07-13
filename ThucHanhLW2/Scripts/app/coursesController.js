var CourseController = function () {
    var btn;
    var init = () => {
        initAttendance();
        $(".js-toggle-attendance").click(toggleAttendance);
    };
    var toggleAttendance = e => {
        btn = $(e.target);
        if (btn.hasClass("btn-default")) createAttendance();
        else deleteAttendance();
    }
    var createAttendance = () => {
        $.post("/api/attendances", { courseId: btn.attr("data-course-id") })
            .done(done)
            .fail(fail);
    }
    var deleteAttendance = () => {
        $.ajax({
            url: "/api/attendances/" + btn.attr("data-course-id"),
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    }
    var initAttendance = () => {
        $.get("/api/attendances")
            .done(data => {
                $(".js-toggle-attendance").each((i, o) => {
                    if (data.filter(a => (a.CourseId + "") === o.getAttribute("data-course-id")).length !== 0) {
                        o.innerText = "Going";
                        o.classList.remove("btn-default");
                        o.classList.add("btn-info");
                    }
                })
            })
    }
    var initMine = () => {
        $(".js-cancel-course").click(e => {
            e.preventDefault();
            var link = $(e.target);
            bootbox.confirm("Are you sure to cancel?",
                confirm => {
                    if (confirm)
                        $.ajax({
                            url: "/api/courses/" + link.attr("data-course-id"),
                            method: "DELETE"
                        })
                            .done(() => doneCourseAction(link))
                            .fail(fail);
                });
        });
    }
    var initCourseUnfollowAction = () => {
        initAttendance();
        $(".js-toggle-attendance").click(e => {
            e.preventDefault();
            var link = $(e.target);
            $.ajax({
                url: "/api/attendances/" + link.attr("data-course-id"),
                method: "DELETE"
            })
                .done(() => doneCourseAction(link))
                .fail(fail);
        });
    }
    var done = () => {
        var text = (btn.text() == "Going") ? "Going?" : "Going";
        btn.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }
    var doneCourseAction = (link) => {
        link.parents("li").fadeOut(() => {
            $(this).remove();
        });
    }
    var fail = (e) => {
        bootbox.alert(typeof e.responseJSON.Message != "undefined" ? e.responseJSON.Message : "Something failed!");
    }
    return {
        init,
        initCourseUnfollowAction,
        initMine
    }
}();