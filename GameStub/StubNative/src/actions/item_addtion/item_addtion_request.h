#ifndef ITEM_ADDTION_REQEUST_H
#define ITEM_ACTION_REQEUST_H
#include <Windows.h>
#include "../action_factory.h"
#include "../../datas/item_addtion_packet.hpp"
#include <iostream>

class ItemAddtionRequest : public ActionFactory
{
private:
public:
	int WINAPI Send(SOCKET s, const char* buf, int len, int flag) override;
	int WINAPI Recv(SOCKET s, char* buf, int len, int flag) override;
};

#endif // !ITEM_ADDTION_REQEUST_H
