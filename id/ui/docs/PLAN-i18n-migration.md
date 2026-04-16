# PLAN: i18n Migration with @ngx-translate

## Overview
Dự án sẽ được chuyển đổi để hỗ trợ đa ngôn ngữ (i18n) với tiếng Việt (vi) và tiếng Anh (en) bằng thư viện `@ngx-translate`. Đồng thời sẽ có một nút Transfer (switch language) ở trên topbar (áp dụng cho các layout in-login/non-login).

**Context:**
- Framework: Angular 11
- Environment: Node 16 (via nvm)
- Target Languages: `vi` (default) & `en`

## Project Type
**WEB** (Primary Agent: `frontend-specialist`)

## 🛑 Socratic Gate (Open Questions for User)
> [!IMPORTANT]
> **Vui lòng trả lời các câu hỏi sau để chốt phạm vi của kế hoạch trước khi bắt đầu code (lệnh `/create`):**
> 1. Trạng thái ngôn ngữ người dùng chọn (vi/en) sẽ được lưu trữ ở đâu? (LocalStorage, hay qua API lưu vào DB của user?)
Trả lời: LocalStorage
> 2. Có cả 2 layout là `in-login` và `non-login`. Topbar switch language có áp dụng hoàn toàn giống nhau trên 2 layout này không?
Trả lời: Có
> 3. Trong quá trình scale sắp tới, chúng ta có cần lazy-load cho các tệp json ngôn ngữ ở từng module không, hay tạm thời chỉ load toàn bộ qua file global (`vi.json`, `en.json`)?
Trả lời: Tạm thời chỉ load toàn bộ qua file global (`vi.json`, `en.json`)

## Success Criteria
- [ ] Ứng dụng tích hợp thành công `@ngx-translate/core` và `@ngx-translate/http-loader` tương thích với Angular 11.
- [ ] Có language switcher trên topbar, thay đổi ngôn ngữ hoạt động tốt tức thì (real-time realtime) mà không cần reload trang.
- [ ] Trạng thái ngôn ngữ được lưu lại để giữ nguyên lựa chọn ở phiên sau.
- [ ] File structure chia nhỏ thư mục tĩnh: `src/assets/i18n/vi.json` và `src/assets/i18n/en.json`.

## Tech Stack
- **@ngx-translate/core@13.x.x**: Phiên bản tương thích với Angular 11.
- **@ngx-translate/http-loader@6.x.x**: Tải file json qua HttpClient từ thư mục `assets`.
- Cần chú ý `npm install` với `--legacy-peer-deps` hoặc `--force` nếu có conflict node 16 và các thư viện cũ của Angular 11.

## File Structure
```
src/
├── assets/
│   └── i18n/
│       ├── en.json       [NEW]
│       └── vi.json       [NEW]
├── app/
│   ├── app.module.ts     [MODIFY] - Setup translate module & provide TranslateHttpLoader
│   ├── app.component.ts  [MODIFY] - Setup default language
│   └── pages/layouts/    
│       ├── in-login/     [MODIFY] - Thêm switch language UI vào header
│       └── non-login/    [MODIFY] - Thêm switch language UI vào header
```

## Task Breakdown

### Task 1: Thiết lập thư viện i18n
- **Agent:** `frontend-specialist`
- **Skill:** `frontend-design`
- **Priority:** P1
- **Input:** Cài đặt các package tương thích Angular 11.
- **Output:** `@ngx-translate/core@13`, `@ngx-translate/http-loader@6` trong `package.json`.
- **Verify:** Chạy `npm install` thành công và không lỗi peer dependency.

### Task 2: Khởi tạo module và file ngôn ngữ tĩnh
- **Agent:** `frontend-specialist`
- **Priority:** P1
- **Dependencies:** Task 1
- **Input:** Import `TranslateModule.forRoot()` trong `app.module.ts`. Tạo `vi.json` và `en.json` tại `assets/i18n/`.
- **Output:** Module biên dịch ngôn ngữ được tích hợp vào core Angular. Set default config ở `app.component.ts`.
- **Verify:** Build lại app không báo lỗi. Test loader có gọi để fetch file json khởi tạo thành công ở tab Network (`F12`).

### Task 3: Phát triển UI Switch Language trên Topbar
- **Agent:** `frontend-specialist`
- **Skill:** `frontend-design`
- **Priority:** P2
- **Dependencies:** Task 2
- **Input:** Các header component layout trong `in-login` và `non-login`.
- **Output:** Có nút select box dropdown (hoặc cờ hiển thị) trên thanh điều hướng topbar trên cả 2 layout. Chỉnh CSS sao cho không làm vỡ các UI layout còn lại.
- **Verify:** Clicks switch sẽ trigger `translateService.use('vi'|'en')`. Text ngoài header bắt đầu thay đổi language. 

### Task 4: Extract Hardcode Text trong Application
- **Agent:** `frontend-specialist`
- **Skill:** `i18n-localization`
- **Priority:** P2
- **Dependencies:** Task 3
- **Input:** Replace vài hardcode text quan trọng tại Header và Sidebar để test.
- **Output:** Sử dụng pipe `{{ 'KEY' | translate }}` và apply các từ khóa này vao cả `en.json` và `vi.json`.
- **Verify:** Từ khóa được hiển thị đúng nội dung vi/en.

### Phase X: Verification (MANDATORY SCRIPT EXECUTION)
- [ ] Lint: Chạy quy trình lint kiểm tra (`npm run lint`).
- [ ] Build: `ng build --prod` không bị crash do missing translation exports.
- [ ] Manual test: Thay đổi ngôn ngữ, reload lại F5 để kiểm chứng browser ghi nhớ localStorage (nếu áp dụng).
