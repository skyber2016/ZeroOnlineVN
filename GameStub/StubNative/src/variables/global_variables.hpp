#ifndef GLOBAL_VARIABLES
#define GLOBAL_VARIABLES

#include <Windows.h>
#include "type_definations.hpp"
#include <winsock.h>

namespace global_variables
{
    inline HMODULE currentModule = NULL;  // Khởi tạo trực tiếp (C++17 trở lên)
    inline SOCKET CurrentSOCKET = NULL;
    inline send_t oSend = nullptr;
    inline recv_t oRecv = nullptr;
    inline connect_t oConnect = nullptr;
    inline const sockaddr* connectName = nullptr;
}

#endif // !GLOBAL_VARIABLES
