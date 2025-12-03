/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_AMEN_BREAK_PLAYLIST = 564305318U;
        static const AkUniqueID PLAY_BGM_PLAYLIST = 904524261U;
        static const AkUniqueID PLAY_EARLY_HIT = 447642349U;
        static const AkUniqueID PLAY_LATE_HIT = 2058610800U;
        static const AkUniqueID PLAY_PERFECT_HIT = 1260131267U;
        static const AkUniqueID PLAY_SLIME_DEATH = 2068608165U;
        static const AkUniqueID STOP_AMEN_BREAK_PLAYLIST = 1969469092U;
        static const AkUniqueID STOP_BGM_PLAYLIST = 2287618571U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace MEDLEY_BGM_CHOICE
        {
            static const AkUniqueID GROUP = 1268045652U;

            namespace STATE
            {
                static const AkUniqueID MARCH = 1173955642U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID WESTERN = 3740536705U;
            } // namespace STATE
        } // namespace MEDLEY_BGM_CHOICE

        namespace PAUSE_CHOICE
        {
            static const AkUniqueID GROUP = 3112673657U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PAUSED = 319258907U;
                static const AkUniqueID UNPAUSED = 1365518790U;
            } // namespace STATE
        } // namespace PAUSE_CHOICE

    } // namespace STATES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINBANK = 2880737896U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
