#include "action_factory.h"
#include <iostream>
#include "../datas/base_request.hpp"
#include "login/login_action.h"
#include "default/default_action.h"
#include "item_addtion/item_addtion_request.h"

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
	if (baseRequest->len == 32 && baseRequest->type == DataType::ItemAddtionReq)
	{
		return new ItemAddtionRequest();
	}
	return new DefaultAction();

}

int WINAPI ActionFactory::Send(SOCKET s, const char* buf, int len, int flag)
{
	return oSend(s, buf, len, flag);
}

int WINAPI ActionFactory::Recv(SOCKET s, char* buf, int len, int flag)
{
	return oRecv(s, buf, len, flag);
}

int WINAPI ActionFactory::CallSend(SOCKET s,const char* buf, int len, int flag)
{
	return oSend(s, buf, len, flag);
}
int WINAPI ActionFactory::CallRecv(SOCKET s, char* buf, int len, int flag)
{
	return oRecv(s, buf, len, flag);
}