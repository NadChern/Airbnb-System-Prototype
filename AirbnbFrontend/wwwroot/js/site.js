document.addEventListener("DOMContentLoaded", function () {
    // Initialize the Slick carousel for property listings
    $('.slick-slider').slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1
                }
            }
        ]
    });
});
