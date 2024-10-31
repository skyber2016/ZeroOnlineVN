#include "hook_managers.h"
namespace hook_managers
{
	void Initialized()
	{
		winsock_hook::Init();
		graphic_hook::Init();
		_isInitialized = true;
	}

	void Destroy()
	{
		if (_isInitialized)
		{
			winsock_hook::Destroy();
			graphic_hook::Destroy();
		}
		_isInitialized = false;
	}
}