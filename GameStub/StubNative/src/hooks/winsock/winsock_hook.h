#ifndef WINSOCK_HOOK_H
#define WINSOCK_HOOK_H

#include <Windows.h>
#include <detours.h>
#include <winsock.h>
#include <iostream>
#include "../../datas/base_request.hpp"
#include "../../actions/action_factory.h"
#include "../../variables/global_variables.hpp"
#include "../../variables/type_definations.hpp"


namespace winsock_hook
{
	void Init();
	void Destroy();
	// Hooked send function
	int WINAPI hooked_send(SOCKET s, const char* buf, int len, int flags);
	// Hooked recv function
	int WINAPI hooked_recv(SOCKET s, char* buf, int len, int flags);
	// Hàm connect hook
	int WINAPI hook_connect(SOCKET s, const sockaddr* name, int namelen);
};

#endif // !WINSOCK_HOOK_H
