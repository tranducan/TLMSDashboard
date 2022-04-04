var src = window.autocompleteData;
var incorrectFlag = false;

(function ($) {
    function checkPasswordMatch() {
        var $passwordForm = $("#update-password-form");
        var password = $passwordForm.find("[name=NewPassword]").first().val();
        var confirmPassword = $passwordForm.find("[name=ConfirmPasswod]").first().val();

        if (password != confirmPassword)
            incorrectFlag = true;
        else
            incorrectFlag = false;
    }

    $(document).on('click', '.pass_show .ptxt', function () {

        $(this).text($(this).text() == "Show" ? "Hide" : "Show");

        $(this).prev().attr('type', function (index, attr) { return attr == 'password' ? 'text' : 'password'; });

    });

    $(document).ready(function () {

        $('input[name="daterange"]').daterangepicker();
        $('.pass_show').append('<span class="ptxt">Show</span>');

        // CONFIRM PASSWORD

        $("#ConfirmPass").keyup(checkPasswordMatch);
        $("#update-password-form [type=submit]").click(function (e) {
            e.preventDefault();

            if (incorrectFlag) {
                alert("Password Confirm Not Match");
            } else {
                $('#update-password-form').submit();
            }
        });

        var form = $("#filter").submit(function (e) {
            e.preventDefault();

            var datePicker = $("#datepicker").data('daterangepicker');
            var startDate = datePicker.startDate.format('YYYY-MM-DD');
            var endDate = datePicker.endDate.format('YYYY-MM-DD');
            var line = $("#linesummary").val();

            var params = $.param({
                StartDate: startDate,
                EndDate: endDate,
                Line: line
            })
            window.location.href = location.protocol + '//' + location.host + "/Summary/Detail" + '?' + params;
        })


        // TOGGLE SIDEBAR

        const reloadTimes = localStorage.getItem("reload-time");

        $("#sidebar").mCustomScrollbar({
            theme: "minimal"
        });

        $('#sidebarCollapse').on('click', function () {
            $('#sidebar, #content').toggleClass('active');
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        });


        // With JQuery
        $("#ex6").slider();

        if ($("#ex6") && reloadTimes) {
            var reloadStr = getReloadTimeStr(reloadTimes)

            $("#ex6SliderVal").text(reloadStr);
            $("#ex6").attr("data-slider-value", reloadTimes);
            $("#ex6").slider('setValue', reloadTimes)
        }

        $("#ex6").on("slide", function (slideEvt) {
            $("#ex6SliderVal").text(`${getReloadTimeStr(slideEvt.value)}`);
            
        });

        function getReloadTimeStr(reloadTimes) {
            var secs = Number.parseInt(reloadTimes) * 1000;
            const now = Date.now();
            const nextReload = now + secs;
            var countDownDate = new Date(nextReload).getTime();

            var distance = countDownDate - now;

            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);


            if (minutes > 0) {
                return `${minutes} mins ${seconds} secs`;
            } else {
                return `${seconds} secs`;
            }
        }

        $(".setting-form").on("submit", function (event) {
            event.preventDefault();
            var formValues = $(this);

            var reloadSettings = $(formValues).find("[name=reload-time]").val();

            localStorage.setItem("reload-time", reloadSettings);

            alert("Success");

        })

        // RELOAD SETTING


        

        //if (isReloadable.length > 0) {
        //    const reloadTimes = localStorage.getItem("reload-time");

        //    if (reloadTimes) {
        //        const reloadInt = Number.parseInt(reloadTimes);
        //        const timeOut = reloadInt * 60 * 1000;

        //        console.log(timeOut);
        //        setTimeout(function () {
        //            location.reload();
        //        }, timeOut)
        //    }
        //}

        // Auto complete search
        
        $('#autocomplete').autocomplete({
            source: src,
            onSelectItem: onSelectItem,
            highlightClass: 'text-danger',
            treshold: 2,
        });

        function onSelectItem(item, element) {
            console.log(item);
            $('#current-goal').val(item.value)
        }

        // COUNTDOWN RELOAD
        const isReloadable = $(".reloadable");
        if (reloadTimes && isReloadable.length > 0) {
            const reloadInt = Number.parseInt(reloadTimes);
            const timeOut = reloadInt * 1000;
            const now = Date.now();
            const nextReload = now + timeOut;

            var countDownDate = new Date(nextReload).getTime();

            var x = setInterval(function () {

                var now = new Date().getTime();

                var distance = countDownDate - now;

                //var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                //var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));

                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                //document.getElementById("demo").innerHTML =
                if (minutes >= 0 || seconds >= 0) {
                    $("#reload-countdown").text(`Reload page after: ${minutes} minutes ${seconds} seconds`)
                } 
                

                if (distance < 0) {
                    clearInterval(x);
                    location.reload();
                }
            }, 1000);
        } else {
            $("#reload-countdown").html("Please setting reloads time")
        }


        // Maptable table
        const mapTable = $(".maptable");
        if (mapTable.length > 0) {
            const provideData = mapTable.attr("data-provider");
            const keyRender = mapTable.find("thead tr th");
            if (keyRender.length > 0) {
                const listKeyRender = keyRender.map(function () {
                    return $(this).attr("data-name")
                })
                    .get();

                console.log(listKeyRender);
                const valueRender = JSON.parse(provideData);
                mapTable.append("<tbody></tbody>");
                const mapBody = mapTable.find("tbody");

                valueRender.forEach(function (value) {
                    const el = $("<tr></tr>");
                    const valueEls = listKeyRender.map(function (key) {
                        return $(`<td>${value[key]}</td>`)
                    })

                    el.append(valueEls);
                    mapBody.append(el);
                })
            }

        }

    });
})(jQuery);