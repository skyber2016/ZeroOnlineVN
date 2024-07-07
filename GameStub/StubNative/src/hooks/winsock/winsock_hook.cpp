#include "winsock_hook.h"
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
		auto ret =  actionExecutor->Send(buf, len, flags);
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
		auto ret = actionExecutor->Recv(buf, len, flags);
		
		delete actionExecutor;
		delete factory;
		return ret;
	}

	// Hàm connect hook
	int WINAPI hook_connect(SOCKET s, const sockaddr* name, int namelen)
	{
		try
		{
			std::cout << "Hooked connect called!" << std::endl;
			CurrentSOCKET = s;
			connectName = name;
			// Gọi hàm connect gốc
			return oConnect(s, name, namelen);
		}
		catch (const std::exception&)
		{
			return 0;
		}

	}
}