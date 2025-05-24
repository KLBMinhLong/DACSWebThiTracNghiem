// Lấy thời gian từ data attribute
let timeLeft = parseInt(
  document.getElementById("countdown").getAttribute("data-time")
);
const countdown = document.getElementById("countdown");
const timer = setInterval(() => {
  let minutes = Math.floor(timeLeft / 60);
  let seconds = timeLeft % 60;
  countdown.textContent = `${minutes}:${seconds.toString().padStart(2, "0")}`;
  // Thêm cảnh báo khi còn 5 phút
  if (timeLeft <= 300 && timeLeft > 0) {
    countdown.classList.add("time-warning");
    document.querySelector(".timer-container").classList.add("time-warning");

    // Hiển thị thông báo chỉ một lần khi còn đúng 5 phút
    if (timeLeft === 300) {
      showTimeWarning();
    }
  }

  timeLeft--;

  if (timeLeft < 0) {
    clearInterval(timer);
    document.querySelector("form[action*='NopBai']").submit();
  }
}, 1000);

// Hiển thị cảnh báo còn 5 phút
function showTimeWarning() {
  const warningDiv = document.createElement("div");
  warningDiv.className =
    "time-alert position-fixed top-50 start-50 translate-middle";
  warningDiv.style.zIndex = "9999";
  warningDiv.style.padding = "20px 40px";
  warningDiv.style.borderRadius = "10px";
  warningDiv.style.boxShadow = "0 5px 15px rgba(0, 0, 0, 0.3)";
  warningDiv.innerHTML = `
        <h4><i class="bi bi-exclamation-triangle-fill me-2"></i>Cảnh báo thời gian!</h4>
        <p class="mb-0">Bạn còn 5 phút để hoàn thành bài thi.</p>
    `;

  document.body.appendChild(warningDiv);

  // Thêm hiệu ứng rung lắc cho cảnh báo
  warningDiv.animate(
    [
      { transform: "translateX(-5px)" },
      { transform: "translateX(5px)" },
      { transform: "translateX(-5px)" },
      { transform: "translateX(5px)" },
      { transform: "translateX(0)" },
    ],
    {
      duration: 500,
      iterations: 2,
    }
  );

  // Tự động ẩn sau 5 giây
  setTimeout(() => {
    warningDiv.style.opacity = "0";
    warningDiv.style.transition = "opacity 1s";

    setTimeout(() => {
      document.body.removeChild(warningDiv);
    }, 1000);
  }, 5000);
}

// AJAX lưu đáp án từng câu
function luuDapAn(lichSuId, cauHoiId, dapAnChon) {
  fetch("/Thi/LuuDapAn", {
    method: "POST",
    headers: { "Content-Type": "application/x-www-form-urlencoded" },
    body: `lichSuId=${lichSuId}&cauHoiId=${cauHoiId}&dapAnChon=${dapAnChon}`,
  }).then((res) => {
    if (!res.ok) {
      alert("Lưu đáp án thất bại!");
    }
  });
}

// Đánh dấu câu trả lời được chọn
function markSelected(input) {
  // Xóa class selected từ tất cả các options trong nhóm
  const answerOptions = input
    .closest(".answer-options")
    .querySelectorAll(".answer-option");
  answerOptions.forEach((option) => option.classList.remove("selected"));

  // Thêm class selected vào option được chọn
  input.closest(".answer-option").classList.add("selected");
}

// Khởi tạo modal và các sự kiện khi DOM đã sẵn sàng
document.addEventListener("DOMContentLoaded", function () {
  // Xác nhận trước khi nộp bài
  const submitButton = document.getElementById("submitButton");
  const confirmModal = document.getElementById("confirmModal");
  const cancelSubmit = document.getElementById("cancelSubmit");
  const confirmSubmit = document.getElementById("confirmSubmit");

  if (submitButton && confirmModal) {
    submitButton.addEventListener("click", function (e) {
      e.preventDefault(); // Ngăn form submit
      confirmModal.style.display = "block";
    });
  }

  if (cancelSubmit) {
    cancelSubmit.addEventListener("click", function () {
      confirmModal.style.display = "none";
    });
  }

  if (confirmSubmit) {
    confirmSubmit.addEventListener("click", function () {
      // Sửa để tìm đúng form nộp bài
      document.querySelector("form[action*='NopBai']").submit();
    });
  }

  // Đóng modal nếu nhấn ra ngoài
  window.addEventListener("click", function (event) {
    if (confirmModal && event.target == confirmModal) {
      confirmModal.style.display = "none";
    }
  });

  // Làm cho header sticky khi cuộn
  window.addEventListener("scroll", function () {
    const header = document.getElementById("exam-header");
    if (header) {
      if (window.scrollY > 10) {
        header.style.boxShadow = "0 4px 10px rgba(0, 0, 0, 0.1)";
      } else {
        header.style.boxShadow = "0 2px 10px rgba(0, 0, 0, 0.1)";
      }
    }
  });
});
