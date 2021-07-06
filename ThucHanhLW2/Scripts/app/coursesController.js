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
    var done = () => {
        var text = (btn.text() == "Going") ? "Going?" : "Going";
        btn.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }
    var fail = () => {
        alert("Something failed!");
    }
    return {
        init: init
    }
}();