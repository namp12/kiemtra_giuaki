// Unified Client Experience Script: 3D Tilt, Shopping Cart, Toast Notifications & Theme Switcher
document.addEventListener('DOMContentLoaded', () => {
    // ----------------------------------------------------
    // 1. Interactive 3D Card Tilt Effect
    // ----------------------------------------------------
    const init3DTilt = () => {
        const cards = document.querySelectorAll('.card-3d');
        cards.forEach(card => {
            card.addEventListener('mousemove', e => {
                const rect = card.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                
                const centerX = rect.width / 2;
                const centerY = rect.height / 2;
                
                const rotateX = ((centerY - y) / centerY) * 8; 
                const rotateY = ((x - centerX) / centerX) * 8;
                
                card.style.transform = `translateY(-10px) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
            });
            
            card.addEventListener('mouseleave', () => {
                card.style.transform = 'translateY(0px) rotateX(0deg) rotateY(0deg)';
            });
        });
    };
    init3DTilt();

    // ----------------------------------------------------
    // 2. Light / Dark Theme Switcher
    // ----------------------------------------------------
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const body = document.body;
    const cartCloseBtn = document.getElementById('cartCloseBtn');

    const updateThemeUI = (isDark) => {
        if (isDark) {
            body.classList.add('dark-theme');
            if (themeIcon) {
                themeIcon.className = 'bi bi-sun-fill text-warning';
            }
            if (cartCloseBtn) {
                cartCloseBtn.classList.add('btn-close-white');
            }
        } else {
            body.classList.remove('dark-theme');
            if (themeIcon) {
                themeIcon.className = 'bi bi-moon-stars-fill text-slate-700';
            }
            if (cartCloseBtn) {
                cartCloseBtn.classList.remove('btn-close-white');
            }
        }
    };

    // Load theme setting from LocalStorage
    let isDarkMode = localStorage.getItem('darkMode') === 'true';
    // If not set, default to light mode
    if (localStorage.getItem('darkMode') === null) {
        isDarkMode = false;
    }
    updateThemeUI(isDarkMode);

    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            isDarkMode = !body.classList.contains('dark-theme');
            updateThemeUI(isDarkMode);
            localStorage.setItem('darkMode', isDarkMode);
            showToast('Chế độ màn hình', `Đã chuyển sang giao diện ${isDarkMode ? 'Tối' : 'Sáng'}!`);
        });
    }

    // ----------------------------------------------------
    // 3. Dynamic Toast Notification System
    // ----------------------------------------------------
    const createToastContainer = () => {
        let container = document.getElementById('toastContainerCustom');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toastContainerCustom';
            container.className = 'toast-container-custom';
            document.body.appendChild(container);
        }
        return container;
    };

    window.showToast = (title, message) => {
        const container = createToastContainer();
        
        const toast = document.createElement('div');
        toast.className = 'toast-custom';
        toast.innerHTML = `
            <div class="toast-custom-content">
                <i class="bi bi-check-circle-fill toast-custom-icon"></i>
                <div>
                    <div class="toast-custom-title">${title}</div>
                    <div class="small text-muted">${message}</div>
                </div>
            </div>
            <button class="toast-custom-close"><i class="bi bi-x"></i></button>
        `;
        
        container.appendChild(toast);
        
        // Trigger show slide in
        setTimeout(() => toast.classList.add('show'), 50);
        
        // Auto remove function
        const removeToast = () => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 400);
        };
        
        // Remove toast on click close
        toast.querySelector('.toast-custom-close').addEventListener('click', removeToast);
        
        // Auto remove after 3s
        setTimeout(removeToast, 3500);
    };

    // ----------------------------------------------------
    // 4. Shopping Cart System (LocalStorage Based)
    // ----------------------------------------------------
    let cart = JSON.parse(localStorage.getItem('futuretech_cart')) || [];

    const saveCart = () => {
        localStorage.setItem('futuretech_cart', JSON.stringify(cart));
        updateCartUI();
    };

    window.addToCart = (id, name, price, image) => {
        const existingItem = cart.find(item => item.id === id);
        if (existingItem) {
            existingItem.qty += 1;
        } else {
            cart.push({ id, name, price, image, qty: 1 });
        }
        saveCart();
        showToast('Giỏ Hàng', `Đã thêm thành công ${name} vào giỏ!`);
    };

    window.updateQty = (id, change) => {
        const item = cart.find(item => item.id === id);
        if (item) {
            item.qty += change;
            if (item.qty <= 0) {
                cart = cart.filter(i => i.id !== id);
            }
            saveCart();
        }
    };

    window.removeFromCart = (id) => {
        const item = cart.find(item => item.id === id);
        cart = cart.filter(i => i.id !== id);
        saveCart();
        if (item) {
            showToast('Giỏ Hàng', `Đã xóa ${item.name} khỏi giỏ hàng!`);
        }
    };

    const updateCartUI = () => {
        // Update badge
        const badge = document.getElementById('cartCountBadge');
        const totalItems = cart.reduce((sum, item) => sum + item.qty, 0);
        
        if (badge) {
            if (totalItems > 0) {
                badge.innerText = totalItems;
                badge.classList.remove('d-none');
            } else {
                badge.classList.add('d-none');
            }
        }

        // Render drawer items
        const listContainer = document.getElementById('cartItemsList');
        const totalSumLabel = document.getElementById('cartTotalSum');
        
        if (!listContainer) return;

        if (cart.length === 0) {
            listContainer.innerHTML = `
                <div class="text-center py-5 text-muted">
                    <i class="bi bi-cart-x fs-1 mb-2 d-block"></i>
                    Giỏ hàng của bạn đang trống.
                </div>
            `;
            if (totalSumLabel) totalSumLabel.innerText = '0đ';
            return;
        }

        let cartHTML = '';
        let totalSum = 0;

        cart.forEach(item => {
            const subtotal = item.price * item.qty;
            totalSum += subtotal;
            
            cartHTML += `
                <div class="cart-item">
                    <img src="${item.image || 'https://images.unsplash.com/photo-1547082299-de196ea013d6?auto=format&fit=crop&w=100&q=80'}" class="cart-item-img" alt="${item.name}" />
                    <div class="cart-item-info">
                        <div class="cart-item-name text-truncate-1">${item.name}</div>
                        <div class="cart-item-price">${item.price.toLocaleString('vi-VN')}đ</div>
                        <div class="cart-item-qty-container">
                            <button class="cart-qty-btn" onclick="updateQty(${item.id}, -1)">-</button>
                            <span class="fw-bold font-heading small px-1">${item.qty}</span>
                            <button class="cart-qty-btn" onclick="updateQty(${item.id}, 1)">+</button>
                        </div>
                    </div>
                    <button class="cart-item-remove" onclick="removeFromCart(${item.id})" title="Xóa mặt hàng">
                        <i class="bi bi-trash-fill"></i>
                    </button>
                </div>
            `;
        });

        listContainer.innerHTML = cartHTML;
        if (totalSumLabel) {
            totalSumLabel.innerText = totalSum.toLocaleString('vi-VN') + 'đ';
        }
    };

    window.checkout = () => {
        if (cart.length === 0) {
            showToast('Lỗi Thanh Toán', 'Giỏ hàng đang trống, không thể thanh toán!');
            return;
        }
        alert('Cảm ơn bạn đã mua sắm tại FutureTech! Đơn hàng của bạn đang được xử lý.');
        cart = [];
        saveCart();
        // Hide offcanvas cart
        const cartDrawerElement = document.getElementById('cartOffcanvas');
        if (cartDrawerElement) {
            const bsOffcanvas = bootstrap.Offcanvas.getInstance(cartDrawerElement);
            if (bsOffcanvas) bsOffcanvas.hide();
        }
    };

    // Initial load
    updateCartUI();
});
