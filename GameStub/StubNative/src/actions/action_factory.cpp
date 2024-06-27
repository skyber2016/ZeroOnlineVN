#include "action_factory.h"
#include <iostream>
#include "../datas/base_request.hpp"
#include "login/login_action.h"
#include "../variables/global_variables.hpp"
#include "default/default_action.h"

using namespace global_variables;

ActionFactory* ActionFactory::GetAction(const char* buf, int len, int flag)
{
	if (len < sizeof(BaseRequest))
	{
		return new DefaultAction;
	}

	BaseRequest* baseRequest = reinterpret_cast<BaseRequest*>((int*)buf);
	if (baseRequest->type == DataType::LoginRequest)
	{
		return new LoginAction();
	}
	return new DefaultAction;

}

int WINAPI ActionFactory::Send(const char* buf, int len, int flag)
{
	return this->CallSend(buf, len, flag);
}

int WINAPI ActionFactory::Recv(char* buf, int len, int flag)
{
	return this->CallRecv(buf, len, flag);
}

int WINAPI ActionFactory::CallSend(const char* buf, int len, int flag)
{
	return oSend(global_variables::CurrentSOCKET, buf, len, flag);
}
int WINAPI ActionFactory::CallRecv(char* buf, int len, int flag)
{
	return oRecv(global_variables::CurrentSOCKET, buf, len, flag);
}