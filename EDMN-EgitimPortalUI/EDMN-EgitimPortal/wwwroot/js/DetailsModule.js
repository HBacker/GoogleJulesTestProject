document.querySelector('.container').classList.replace('container', 'HBR');


const videoElement = document.getElementById('courseVideo');

// Video sürelerini hesapla ve göster
function calculateDuration(videoSrc, durationElement) {
    const tempVideo = document.createElement('video');
    tempVideo.src = videoSrc;

    tempVideo.addEventListener('loadedmetadata', function () {
        const duration = Math.round(tempVideo.duration);
        const minutes = Math.floor(duration / 60);
        const seconds = duration % 60;
        durationElement.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
    });

    tempVideo.addEventListener('error', function () {
        durationElement.textContent = '--:--';
    });
}

// Sayfa yüklendiğinde tüm video sürelerini hesapla
document.querySelectorAll('.lesson-item').forEach(item => {
    const videoSrc = item.dataset.video;
    const durationSpan = item.querySelector('.lesson-duration');
    if (videoSrc) {
        calculateDuration(videoSrc, durationSpan);
    }
});

// Video değiştirme işlevi
document.querySelectorAll('.lesson-item').forEach(item => {
    item.querySelector('.lesson-link').addEventListener('click', function (event) {
        event.preventDefault();

        // Tüm active class'ları kaldır
        document.querySelectorAll('.lesson-item').forEach(lessonItem => {
            lessonItem.classList.remove('active');
        });

        // Tıklanan öğeye active class ekle
        item.classList.add('active');

        // Video kaynağını güncelle
        const videoSrc = item.dataset.video;
        if (videoSrc && videoElement) {
            const videoSource = videoElement.querySelector('source');
            videoSource.src = videoSrc;
            videoElement.load();
            videoElement.play();
        }
    });
});