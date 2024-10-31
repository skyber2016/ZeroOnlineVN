#include "winsock_hook.h"

#pragma comment(lib, "WS2_32.Lib")

using namespace global_variables;
namespace winsock_hook
{
	void Init()
	{
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		// Hook send and recv
		oConnect = (connect_t)DetourFindFunction("Ws2_32.dll", "connect");
		oSend = (send_t)DetourFindFunction("Ws2_32.dll", "send");
		oRecv = (recv_t)DetourFindFunction("Ws2_32.dll", "recv");

		DetourAttach(&(PVOID&)oConnect, hook_connect);
		DetourAttach(&(PVOID&)oSend, hooked_send);
		DetourAttach(&(PVOID&)oRecv, hooked_recv);

		DetourTransactionCommit();
	}
	void Destroy()
	{
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());

		DetourDetach(&(PVOID&)oSend, hooked_send);
		DetourDetach(&(PVOID&)oRecv, hooked_recv);
		DetourDetach(&(PVOID&)oConnect, hook_connect);

		DetourTransactionCommit();
	}
	// Hooked send function
	int WINAPI hooked_send(SOCKET s, const char* buf, int len, int flags)
	{
		if (!CurrentSOCKET)
		{
			CurrentSOCKET = s;
		}
		ActionFactory* factory = new ActionFactory();
		ActionFactory* actionExecutor = factory->GetAction(buf, len, flags);
		auto ret =  actionExecutor->Send(s, buf, len, flags);
		delete actionExecutor;
		delete factory;
		return ret;
	}

	// Hooked recv function
	int WINAPI hooked_recv(SOCKET s, char* buf, int len, int flags)
	{
		if (!CurrentSOCKET)
		{
			CurrentSOCKET = s;
		}
		ActionFactory* factory = new ActionFactory();
		ActionFactory* actionExecutor = factory->GetAction(buf, len, flags);
		auto ret = actionExecutor->Recv(s, buf, len, flags);
		
		delete actionExecutor;
		delete factory;
		return ret;
	}

	// Hàm connect hook
	int WINAPI hook_connect(SOCKET socket, const sockaddr* name, int namelen)
	{
		return oConnect(socket, name, namelen);
	}
}