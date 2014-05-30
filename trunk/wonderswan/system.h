#ifndef SYSTEM_H
#define SYSTEM_H

namespace MDFN_IEN_WSWAN
{
class System;
struct SyncSettings;
struct Settings;
}

#include "wswan.h"
#include "gfx.h"
#include "memory.h"
#include "eeprom.h"
#include "rtc.h"
#include "sound.h"
#include "v30mz.h"
#include "interrupt.h"

#include <cstddef>

namespace MDFN_IEN_WSWAN
{
class System
{
public:
	System();
	~System();

	static void* operator new(std::size_t size);

	void Reset();
	bool Advance(uint16 buttons, bool novideo, uint32 *surface, int16 *soundbuff, int &soundbuffsize);
	bool Load(const uint8 *data, int length, const SyncSettings &s);

	int SaveRamSize() const;
	bool SaveRamLoad(const uint8 *data, int size);
	bool SaveRamSave(uint8 *dest, int maxsize) const;

public:
	GFX gfx;
	Memory memory;
	EEPROM eeprom;
	RTC rtc;
	Sound sound;
	V30MZ cpu;
	Interrupt interrupt;
	
	bool rotate; // rotate screen and controls left 90
};

struct SyncSettings
{
	uint16 byear; // birth year, 0000-9999
	uint8 bmonth; // birth month, 1-12
	uint8 bday; // birth day, 1-31
	char name[17]; // up to 16 chars long, most chars don't work (conversion from ascii is internal)
	uint8 language; // 0 = J, 1 = E; only affects "Digimon Tamers - Battle Spirit"
	uint8 sex; // sex, 1 = male, 2 = female
	uint8 blood; // 1 = a, 2 = b, 3 = o, 4 = ab
	bool color; // true if wonderswan is in color mode
	bool userealtime; // true to use the system's actual clock; false to use an emulation pegged clock
	uint64 initialtime; // when userealtime is false, the initial time in unix format
};

struct Settings
{
	uint32 LayerMask; // 1 = enable bg, 2 = enable fg, 4 = enable sprites
};

namespace Debug
{
int puts ( const char * str );
int printf ( const char * format, ... );
}

}

#endif