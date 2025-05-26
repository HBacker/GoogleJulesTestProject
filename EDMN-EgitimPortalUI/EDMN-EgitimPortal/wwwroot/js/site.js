


//Carousel Actions
document.addEventListener("DOMContentLoaded", function () {
    const slides = document.querySelectorAll(".main-carousel-slide");
    const prevButton = document.querySelector(".main-carousel-prev");
    const nextButton = document.querySelector(".main-carousel-next");
    let currentIndex = 0;

    function goToNextSlide() {
        slides[currentIndex].classList.remove("active");
        currentIndex = (currentIndex + 1) % slides.length;
        slides[currentIndex].classList.add("active");
    }

    function goToPreviousSlide() {
        slides[currentIndex].classList.remove("active");
        currentIndex = (currentIndex - 1 + slides.length) % slides.length;
        slides[currentIndex].classList.add("active");
    }

    nextButton.addEventListener("click", goToNextSlide);
    prevButton.addEventListener("click", goToPreviousSlide);

    setInterval(goToNextSlide, 5000);
})

//Searchbox Enter Event
document.querySelector('.custom-searchbox').addEventListener('submit', function (e) {
    const inputField = document.querySelector('.search-input');
    if (inputField && inputField.value.trim() === "") {
        e.preventDefault();
        alert('Lütfen bir arama terimi girin');
    }
});
