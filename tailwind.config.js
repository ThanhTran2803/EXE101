
module.exports = {
    content: [
        './Pages/**/*.{cshtml,razor}',
        './Views/**/*.{cshtml,razor}', // Đường dẫn tới các tệp sử dụng Tailwind CSS
    ],
    theme: {
        extend: {
            colors: {
                primary: '#512da8',      // Màu nút và các thành phần liên quan
                secondary: '#5c6bc0',    // Màu phụ gradient
                background: '#c9d6ff',   // Màu nền cho body
                gradientStart: '#e2e2e2',// Điểm bắt đầu gradient
                gradientEnd: '#c9d6ff',  // Điểm kết thúc gradient
                lightGray: '#eee',       // Màu cho input
                white: '#ffffff',        // Màu nền cho container
                dark: '#333',            // Màu văn bản và đường viền
            },
            fontSize: {
                sm: '12px',              // Font-size cho span và button
                base: '13px',            // Font-size cho các liên kết và input
                lg: '14px',              // Font-size cho paragraph
            },
            letterSpacing: {
                wider: '0.3px',          // Khoảng cách chữ cho văn bản paragraph
                widest: '0.5px',         // Khoảng cách chữ cho button
            },
            lineHeight: {
                relaxed: '20px',         // Khoảng cách dòng cho paragraph
            },
            borderRadius: {
                xl: '30px',              // Bo góc cho container
                lg: '8px',               // Bo góc cho button và input
                full: '150px',           // Bo góc cho toggle-container
            },
            boxShadow: {
                card: '0 5px 15px rgba(0, 0, 0, 0.35)', // Shadow cho container
            },
            spacing: {
                '10px': '10px',          // Khoảng cách padding, margin tuỳ chỉnh
                '15px': '15px',
                '20px': '20px',
                '45px': '45px',
                '40px': '40px',
            },
            width: {
                'full': '100%',          // Chiều rộng đầy đủ
                '768px': '768px',        // Chiều rộng cố định của container
                '50%': '50%',            // Chiều rộng cho các form sign-in và sign-up
                'auto': 'auto',          // Cho các nút button
                '40px': '40px',          // Chiều rộng cho các biểu tượng mạng xã hội
            },
            height: {
                'full': '100%',          // Chiều cao đầy đủ
                '500px': '500px',        // Chiều cao cho container
                '40px': '40px',          // Chiều cao cho các biểu tượng mạng xã hội
                '100vh': '100vh',        // Chiều cao toàn màn hình cho body
            },
            keyframes: {
                move: {
                    '0%, 49.99%': { opacity: 0, zIndex: 1 },
                    '50%, 100%': { opacity: 1, zIndex: 5 },
                },
            },
            animation: {
                move: 'move 0.6s ease-in-out', // Animation cho sign-up
            },
            zIndex: {
                '1': '1',
                '2': '2',
                '5': '5',
                '1000': '1000',
            },
            transitionTimingFunction: {
                'ease-in-out': 'ease-in-out', // Tùy chọn easing cho các transition
            },
            transitionDuration: {
                '600': '600ms', // Thời gian chuyển đổi
            },
        },
    },
    plugins: [
        require('@tailwindcss/forms'), // Plugin cho input form
    ],
};
