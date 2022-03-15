var src = window.autocompleteData;

(function($) {
    $(document).ready(function() {
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
        $("#ex6").on("slide", function (slideEvt) {
            $("#ex6SliderVal").text(slideEvt.value);
        });

        $(".setting-form").on("submit", function (event) {
            event.preventDefault();
            var formValues = $(this);

            var reloadSettings = $(formValues).find("[name=reload-time]").val();

            localStorage.setItem("reload-time", reloadSettings);

            alert("Success");

        })

        const isReloadable = $(".reloadable");

        if (isReloadable.length > 0) {
            const reloadTimes = localStorage.getItem("reload-time");

            if (reloadTimes) {
                const reloadInt = Number.parseInt(reloadTimes);
                const timeOut = reloadInt * 60 * 1000;

                console.log(timeOut);
                setTimeout(function () {
                    location.reload();
                }, timeOut)
            }
        }

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


    });
})(jQuery);