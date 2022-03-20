var src = window.autocompleteData;

(function($) {
    $(document).ready(function() {
      
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
        if (reloadTimes && isReloadable) {
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


        


    });
})(jQuery);