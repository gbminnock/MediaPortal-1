/* 
 *  Copyright (C) 2006-2008 Team MediaPortal
 *  http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
#pragma once

#include "..\..\shared\sectiondecoder.h"
#include <map>
using namespace std;

class IPatCallBack
{
  public:
    virtual void OnPatReceived(int serviceId, int pmtPid) = 0;
};

#define PID_PAT 0x0

class CPatParser : public CSectionDecoder
{
  public:
    CPatParser(void);
    virtual ~CPatParser(void);
    void Reset();
    void SetCallBack(IPatCallBack* callBack);
    void OnNewSection(CSection& sections);
    bool IsReady();
    int GetServiceCount();
    int GetService(int idx, int* serviceId, int* pmtPid);
    int GetPmtPid(int serviceId, int* pmtPid);

  private:
    IPatCallBack* m_pCallBack;
    map<int, bool> m_mSeenSections;
    bool m_bIsReady;
    map<int, int> m_mServices;
};
