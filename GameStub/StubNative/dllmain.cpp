#pragma comment(lib, "d3d11.lib")
#pragma comment(lib, "dxgi.lib")

#include "pch.h"
void initConsole()
{
    if (GetConsoleWindow() == NULL)
    {
        AllocConsole();
        FILE* f;
        freopen_s(&f, "CONOUT$", "w", stdout);
        freopen_s(&f, "CONOUT$", "w", stderr);
        std::cout.clear();
        std::cin.clear();
    }
}



DWORD WINAPI MainThread(LPVOID lpParam)
{
    initConsole();
    hook_managers::Initialized();
    while (true)
    {
        // Kiểm tra nếu phím F1 được nhấn
        if (GetAsyncKeyState(VK_F1) & 0x8000)
        {
            std::cout << "F1 key pressed. Exiting thread and freeing library." << std::endl;
            hook_managers::Destroy();
            FreeConsole();
            FreeLibraryAndExitThread(global_variables::currentModule, 0);
        }
        Sleep(100);  // Tạm dừng để giảm tải CPU
    }

    return 0;
}

extern "C" __declspec(dllexport) BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    {
        global_variables::currentModule = hModule;
        DisableThreadLibraryCalls(hModule);
        HANDLE threadHandle = CreateThread(
            NULL,                   // Không có thuộc tính bảo mật
            0,                      // Kích thước ngăn xếp mặc định
            MainThread,             // Hàm thực thi của thread
            NULL,                   // Không có đối số cho hàm thread
            0,                      // Sử dụng các cài đặt mặc định
            NULL                    // ID của thread sẽ được lưu vào đây
        );

        // Kiểm tra thread có được tạo thành công hay không
        if (threadHandle)
        {
            CloseHandle(threadHandle);
        }

        

        break;
    }
    case DLL_THREAD_DETACH:

        break;
    }
    return TRUE;
}



