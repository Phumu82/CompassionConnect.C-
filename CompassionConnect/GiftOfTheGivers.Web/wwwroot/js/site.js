// Gift of the Givers - site-wide behaviour
(function () {
    "use strict";

    // Highlight the active nav link based on current path
    document.querySelectorAll(".gg-nav .nav-link").forEach(function (link) {
        if (link.getAttribute("href") === window.location.pathname) {
            link.classList.add("active");
        }
    });

    // Auto-dismiss alerts after 6 seconds
    document.querySelectorAll(".alert").forEach(function (alertEl) {
        setTimeout(function () {
            var bsAlert = bootstrap.Alert.getOrCreateInstance(alertEl);
            bsAlert.close();
        }, 6000);
    });

    // Donation amount quick-select buttons
    document.querySelectorAll("[data-amount-preset]").forEach(function (btn) {
        btn.addEventListener("click", function () {
            var input = document.getElementById("Amount");
            if (input) {
                input.value = btn.getAttribute("data-amount-preset");
                document.querySelectorAll("[data-amount-preset]").forEach(function (b) {
                    b.classList.remove("btn-primary");
                    b.classList.add("btn-outline-primary");
                });
                btn.classList.remove("btn-outline-primary");
                btn.classList.add("btn-primary");
            }
        });
    });
})();

