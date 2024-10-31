#ifndef ITEM_ADDTION_PACKET_H
#define ITEM_ADDTION_PACKET_H
#include "data_type.hpp"

struct ItemAddtionPacket
{
	unsigned short len;
	DataType type;
	int FirstItemId;
	int SecondItemId;
};

#endif // !ITEM_ADDTION_PACKET_H
