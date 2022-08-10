#ifndef SR7LINK__H
#define SR7LINK__H

#include <stdio.h>

#ifdef WIN32
#define  SR7_IF_API __declspec(dllexport)
#else
#define  SR7_IF_API extern
#endif

typedef void * SR7IF_Data;

/// \brief                      高速数据通信的回调函数接口.
///	\param pBuffer              指向储存概要数据的缓冲区的指针.
///	\param dwSize               每个单元(行)的字节数量.
///	\param dwCount              存储在pBuffer中的内存的单元数量.
///	\param dwNotify             中断或批量结束等中断的通知.
///	\param dwDeviceId           被调用函数DeviceID号.
///
typedef void (*SR7IF_CALLBACK)(char* pBuffer, unsigned int dwSize, unsigned int dwCount, unsigned int dwNotify, unsigned int dwDeviceId);
typedef void (*SR7IF_BatchOneTimeCallBack)(const void *info, const SR7IF_Data *data);

typedef struct {
    unsigned char	abyIpAddress[4];
} SR7IF_ETHERNET_CONFIG;

#define SR7IF_ERROR_NOT_FOUND                     (-999)                  // 功能（相机）不存在.
#define SR7IF_ERROR_COMMAND                       (-998)                  // 该命令不支持.
#define SR7IF_ERROR_PARAMETER                     (-997)                  // 参数错误.
#define SR7IF_ERROR_UNIMPLEMENTED                 (-996)                  // 功能未实现.
#define SR7IF_ERROR_HANDLE                        (-995)                  // 句柄无效.
#define SR7IF_ERROR_MEMORY                        (-994)                  // 内存（溢出/定义）错误.
#define SR7IF_ERROR_TIMEOUT                       (-993)                  // 操作超时.
#define SR7IF_ERROR_DATABUFFER                    (-992)                  // 数据大缓冲区不足.
#define SR7IF_ERROR_STREAM                        (-991)                  // 数据流错误.
#define SR7IF_ERROR_CLOSED                        (-990)                  // 接口关闭不可用.
#define SR7IF_ERROR_VERSION                       (-989)                  // 当前版本无效.
#define SR7IF_ERROR_ABORT                         (-988)                  // 操作被终止，如批处理过程软件终止、连接被关闭、连接中断等.
#define SR7IF_ERROR_ALREADY_EXISTS                (-987)                  // 操作和现有的设置冲突.
#define SR7IF_ERROR_FRAME_LOSS                    (-986)                  // 批处理帧丢失.
#define SR7IF_ERROR_ROLL_DATA_OVERFLOW            (-985)                  // 无终止循环批处理出现溢出异常等.
#define SR7IF_ERROR_ROLL_BUSY                     (-984)                  // 无终止循环批处理读数据忙.
#define SR7IF_ERROR_MODE                          (-983)                  // 当前处理函数与设置的批处理模式有冲突.
#define SR7IF_ERROR_CAMERA_NOT_ONLINE             (-982)                  // 相机（传感头）不在线.
#define SR7IF_ERROR                               (-1)                    // 一般性错误,如链接失败、设置失败、数据获取失败等.
#define SR7IF_NORMAL_STOP                         (-100)                  // 正常停止,如外部IO停止批处理操作等.
#define SR7IF_OK                                  (0)                     // 正确操作.

#ifdef __cplusplus
extern "C" {
#endif

///
/// \brief SR7IF_EthernetOpen   通信连接.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pEthernetConfig      Ethernet 通信设定.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_EthernetOpen(unsigned int lDeviceId, SR7IF_ETHERNET_CONFIG* pEthernetConfig);

///
/// \brief SR7IF_CommClose      断开与相机的连接.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_CommClose(unsigned int lDeviceId);

///
/// \brief SR7IF_SwitchProgram  切换相机配置的参数.重启后不保存配方号.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param No:                  任务参数列表编号 0 - 63.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SwitchProgram(unsigned int lDeviceId, int No);

///
/// \brief SR7IF_GetOnlineCameraB   获取传感头B是否在线
/// \param lDeviceId            设备ID号，范围为0-63.
/// \return
///     <0:                     -982:传感头B不在线
///                             其他:获取失败
///     =0:                     传感头B在线
///
SR7_IF_API int SR7IF_GetOnlineCameraB(unsigned int lDeviceId);


///
/// \brief SR7IF_StartMeasure   开始批处理,立即执行批处理程序.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Timeout              非循环获取时,超时时间(单位ms),-1为无限等待;循环模式该参数可设置为-1.
/// \return
///     <0:                     失败
///     =0:                     成功
///
SR7_IF_API int SR7IF_StartMeasure(unsigned int lDeviceId, int Timeout = 50000);


/// \brief SR7IF_StartIOTriggerMeasure 开始批处理,硬件IO触发开始批处理，具体查看硬件手册.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Timeout              非循环获取时,超时时间(单位ms),-1为无限等待;循环模式该参数可设置为-1.
/// \param restart              预留，设为0.
/// \return
///     <0:                     失败
///     =0:                     成功
///
SR7_IF_API int SR7IF_StartIOTriggerMeasure(unsigned int lDeviceId, int Timeout = 50000, int restart = 0);

///
/// \brief SR7IF_StopMeasure    停止批处理
/// \param lDeviceId            设备ID号，范围为0-63.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_StopMeasure(unsigned int lDeviceId);

/// \brief SR7IF_ReceiveData    阻塞方式获取数据.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              返回数据指针.
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_ReceiveData(unsigned int lDeviceId, SR7IF_Data DataObj);


/// \brief SR7IF_ProfilePointSetCount 获取当前批处理设定行数
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \return                     返回实际批处理行数
///
SR7_IF_API int SR7IF_ProfilePointSetCount(unsigned int lDeviceId, const SR7IF_Data DataObj);

/// \brief SR7IF_ProfilePointCount 获取批处理实际获取行数.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL.
/// \return                     返回批处理实际获取行数.
///
SR7_IF_API int SR7IF_ProfilePointCount(unsigned int lDeviceId, const SR7IF_Data DataObj);

/// \brief SR7IF_ProfileDataWidth 获取数据宽度.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL.
/// \return                     返回数据宽度(单位像素).
///
SR7_IF_API int SR7IF_ProfileDataWidth(unsigned int lDeviceId, const SR7IF_Data DataObj);

///
/// \brief SR7IF_ProfileData_XPitch 获取数据x方向间距.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL.
/// \return                     返回数据x方向间距(mm).
///
SR7_IF_API double SR7IF_ProfileData_XPitch(unsigned int lDeviceId, const SR7IF_Data DataObj);

///
/// \brief SR7IF_GetEncoder     获取编码器值
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Encoder              返回数据指针,双相机为A/B交替数据
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetEncoder(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int *Encoder);

///
/// \brief SR7IF_GetEncoderContiune 非阻塞方式获取编码器值
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Encoder              返回数据指针,双相机为A/B交替数据
/// \param GetCnt               获取数据长度
/// \return
///     <0:                     获取失败.
///     >=0:                    实际返回的数据长度.
///
SR7_IF_API int SR7IF_GetEncoderContiune(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int *Encoder, unsigned int GetCnt);


///
/// \brief SR7IF_GetProfileData 阻塞方式获取轮廓数据
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Profile              返回数据指针,双相机为A/B行交替数据
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetProfileData(unsigned int lDeviceId, const SR7IF_Data DataObj, int *Profile);

///
/// \brief SR7IF_GetProfileContiuneData 非阻塞方式获取轮廓数据
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Profile              返回数据指针,双相机为A/B行交替数据
/// \param GetCnt               获取数据长度
/// \return
///     <0:                     获取失败.
///     >=0:                    实际返回的数据长度.
///Q
SR7_IF_API int SR7IF_GetProfileContiuneData(unsigned int lDeviceId, const SR7IF_Data DataObj, int *Profile, unsigned int GetCnt);



///
/// \brief SR7IF_GetIntensityData  阻塞方式获取亮度数据
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Intensity            返回数据指针,双相机为A/B行交替数据
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetIntensityData(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned char *Intensity);


///
/// \brief SR7IF_GetIntensityContiuneData 非阻塞获取亮度数据
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Intensity            返回数据指针,双相机为A/B行交替数据
/// \param GetCnt               获取数据长度
/// \return
///     <0:                     获取失败.
///     >=0:                    返回获实际数据行数.
///
SR7_IF_API int SR7IF_GetIntensityContiuneData(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned char *Intensity, unsigned int GetCnt);

///
/// \brief SR7IF_GetBatchRollData 无终止循环获取数据
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param Profile              返回轮廓数据指针,双相机为A/B行交替数据
/// \param Intensity            返回亮度数据指针,双相机为A/B行交替数据
/// \param Encoder              返回编码器数据指针,双相机为A/B交替数据
/// \param FrameId              返回帧编号数据指针
/// \param FrameLoss            返回批处理过快掉帧数量数据指针,双相机为A/B交替数据
/// \param GetCnt               获取数据长度
/// \return
///     <0:                     获取失败.
///     >=0:                    实际返回的数据长度.
///
SR7_IF_API int SR7IF_GetBatchRollData(unsigned int lDeviceId, const SR7IF_Data DataObj,
                                        int *Profile, unsigned char *Intensity, unsigned int *Encoder, long long *FrameId, unsigned int *FrameLoss,
                                        unsigned int GetCnt);


///
/// \brief SR7IF_SetBatchRollProfilePoint 无终止循环设定行数
///                               用于数量要求(15000-65535)行且数据速度较快传输速度不足导致覆盖问题
///                               使用该功能初始化时至少设置一次且掉电不保存
/// \param lDeviceId              设备ID号，范围为0-63.
/// \param DataObj                预留，设置为NULL
/// \param points                 0:无终止循环  >=15000:设定行数  其他无效
/// \return
///     <0:                       失败.
///     >=0:                      成功.
///
SR7_IF_API int SR7IF_SetBatchRollProfilePoint(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int points);



///
/// \brief SR7IF_GetBatchRollError   无终止循环获取数据异常计算值
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param EthErrCnt            返回网络传输导致错误的数量
/// \param UserErrCnt           返回用户获取导致错误的数量
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetBatchRollError(unsigned int lDeviceId, int *EthErrCnt, int *UserErrCnt);


///
/// \brief SR7IF_RollDataCallback SR7IF_RollDataCallbackInitalize无终止循环回调行数.
/// \param pProfileBuffer       返回轮廓数据
/// \param pIntensityBuffer     返回灰度数据
/// \param pEncoder             返回编码器数据
/// \param dwSize               数据宽度
/// \param dwCount              批处理点数
/// \param dwRet                返回值，待使用
/// \param dwDeviceId           相应的控制器(0-63)
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
typedef void (*SR7IF_RollDataCallback)(int* pProfileBuffer, unsigned char* pIntensityBuffer, unsigned int* pEncoder,
                                       unsigned int dwSize, unsigned int dwCount, int dwRet, unsigned int dwDeviceId);

///
/// \brief SR7IF_RollDataCallbackInitalize 无终止循环回调方式.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pCallBack            回调函数
/// \param dwProfileCnt         单次回调行数
/// \param pCallBack            获取档次回调行数超时时间，单位ms   <=0:关闭超时
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_RollDataCallbackInitalize(unsigned int lDeviceId, SR7IF_RollDataCallback pCallBack, unsigned int dwProfileCnt, int Timeout);



///
/// \brief SR7IF_SetBatchCtrlByIO   续传功能使能.
///                                 设置相机在一次批处理过程中，可以通过IO控制来暂停或继续批处理
///                                 IO控制使用控制器11脚和14脚配合使用，其中11脚开启电平控制模式，14脚控制暂停和继续批处理
/// \param lDeviceId            设备ID号,范围为0-63.
/// \param DataObj              预留,设置为NULL
/// \param Enable               功能使能0：关闭  1:使能
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SetBatchCtrlByIO(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int Enable);

///
/// \brief SR7IF_GetError       获取系统错误信息
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pbyErrCnt            返回错误码数量
/// \param pwErrCode            返回错误码指针,数组大小建议2048
/// \return
///     <0:                     获取失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetError(unsigned int lDeviceId, int *pbyErrCnt, int *pwErrCode);

///
/// \brief SR7IF_ClearError     暂无
/// \param lDeviceId
/// \param wErrCode
/// \return
///     <0:                     清除失败
///     =0:                     成功
///
SR7_IF_API int SR7IF_ClearError(unsigned int lDeviceId, unsigned short wErrCode);

///
/// \brief SR7IF_GetVersion     获取库版本号.
/// \return                     返回版本信息.
///
SR7_IF_API const char *SR7IF_GetVersion();

///
/// \brief SR7IF_GetModels      获取相机型号.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \return                     返回相机型号字符串.
///
SR7_IF_API const char *SR7IF_GetModels(unsigned int lDeviceId);

///
/// \brief SR7IF_GetHeaderSerial   获取相机头序列号
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Head                 0：相机头A  1：相机头B
/// \return
///     !=NULL:                 返回相机序列号字符串.
///     =NULL:                  失败，相应头不存在或者参数错误.
///
SR7_IF_API const char *SR7IF_GetHeaderSerial(unsigned int lDeviceId, int Head);

/// 高速数据通信相关
///
/// \brief SR7IF_HighSpeedDataEthernetCommunicationInitalize 初始化以太网高速数据通信.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pEthernetConfig      Ethernet 通信设定.
/// \param wHighSpeedPortNo     Ethernet 通信端口设定.
/// \param pCallBack            高速通信中数据接收的回调函数.
/// \param dwProfileCnt         回调函数被调用的频率. 范围1-256
/// \param dwThreadId           线程号.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_HighSpeedDataEthernetCommunicationInitalize(unsigned int lDeviceId, SR7IF_ETHERNET_CONFIG* pEthernetConfig, int wHighSpeedPortNo,
    SR7IF_CALLBACK pCallBack, unsigned int dwProfileCnt, unsigned int dwThreadId);


///
/// \brief SR7IF_SetOutputPortLevel      设置输出端口电平.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Port                 输出端口号，范围为0-7.
/// \param Level                输出电平值.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SetOutputPortLevel(unsigned int lDeviceId, unsigned int Port, bool Level);


///
/// \brief SR7IF_SetOutputPortLevel      读取输入端口电平.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Port                 输入端口号，范围为0-7.
/// \param Level                读取输入电平.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetInputPortLevel(unsigned int lDeviceId, unsigned int Port, bool *Level);

///
/// \brief SR7IF_GetSingleProfile   获取当前一条轮廓（非批处理下,需在EdgeImaging中设置为2.5D模式）
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pProfileData         返回轮廓的指针.
/// \param pEncoder             返回编码器的指针.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetSingleProfile(unsigned int lDeviceId, int *pProfileData, unsigned int *pEncoder);

///
/// \brief SR7IF_SetSetting     参数设定.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Depth                设置的值的级别.
/// \param Type                 设置类型.-1:为设置当前配方参数  0x10-0x50:为配方0-63号配方
/// \param Category             设置种类.
/// \param Item                 设置项目.
/// \param Target[4]            根据发送 / 接收的设定，可能需要进行相应的指定。无需设定时，指定为 0。
/// \param pData                设置数据.
/// \param DataSize             设置数据的长度.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SetSetting(unsigned int lDeviceId, int Depth, int Type, int Category, int Item, int Target[4], void *pData, int DataSize);


///
/// \brief SR7IF_GetSetting     获取参数设定.当获取的配方号为非当前运行的配方时，会导致批处理中断
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param Type                 获取类型. -1:为获取当前配方参数  0x10-0x50:为配方0-63号配方
/// \param Category             获取种类.
/// \param Item                 获取项目.
/// \param Target[4]            根据发送 / 接收的设定，可能需要进行相应的指定。无需设定时，指定为 0。
/// \param pData                获取的数据.
/// \param DataSize             获取数据的长度.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetSetting(unsigned int lDeviceId, int Type, int Category, int Item, int Target[4], void *pData, int DataSize);
///
/// \brief SR7IF_ExportParameters   将系统参数导出，注意只导出当前任务的参数.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param size                 返回参数表的大小.
/// \return
///     NULL:                   失败.
///     其他:                    成功.
///
SR7_IF_API const char *SR7IF_ExportParameters(unsigned int lDeviceId, unsigned int *size);

///
/// \brief SR7IF_LoadParameters   将导出的参数导入到系统中.
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param pSettingdata         导入参数表指针.
/// \param size                 导入参数表的大小.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_LoadParameters(unsigned int lDeviceId, const char *pSettingdata, unsigned int size);

///
/// \brief SR7IF_GetLicenseKey   返回产品剩余天数
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param RemainDay            返回剩余天数
/// \return
///     < 0:                     失败，参数错误或产品未注册
///     >=0:                     成功.
///
SR7_IF_API int SR7IF_GetLicenseKey(unsigned int lDeviceId, unsigned short *RemainDay);

///
/// \brief SR7IF_GetCurrentEncoder   读取当前编码器值
/// \param value                    返回编码器值
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetCurrentEncoder(unsigned int lDeviceId, unsigned int *value);


///
/// \brief SR7IF_GetCameraTemperature   读取相机温度,单位0.01摄氏度
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param tempA                        相机A温度值, -1000000:读温度不支持
/// \param tempB                        相机B温度值, -1000000:读温度不支持
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetCameraTemperature(unsigned int lDeviceId, int *tempA, int *tempB);

///
/// \brief SR7IF_GetCameraBoardTemperature   读取相机主板温度,单位1摄氏度
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param tempA                        相机A温度值, -1000000:读温度不支持
/// \param tempB                        相机B温度值, -1000000:读温度不支持
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetCameraBoardTemperature(unsigned int lDeviceId, int *tempA, int *tempB);

//
typedef struct {
    int xPoints;                //x方向数据数量
    int BatchPoints;            //批处理数量
    unsigned int BatchTimes;    //批处理次数

    double xPixth;              //x方向点间距
    unsigned int startEncoder;  //批处理开始编码器值
    int HeadNumber;             //相机头数量
    int returnStatus;           //SR7IF_OK:正常批处理
                                //SR7IF_NORMAL_STOP
                                //SR7IF_ERROR_ABORT
                                //SR7IF_ERROR_CLOSED
} SR7IF_STR_CALLBACK_INFO;
///
/// \brief SR7IF_SetBatchOneTimeDataHandler   设置回调函数，建议获取数据后另外开启线程进行处理（获取数据模式:批处理一次回调一次）
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param CallFunc             回调函数.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SetBatchOneTimeDataHandler(unsigned int lDeviceId, SR7IF_BatchOneTimeCallBack CallFunc);

///
/// \brief SR7IF_StartMeasureWithCallback   开始批处理（获取数据模式:批处理一次回调一次）
/// \param lDeviceId            设备ID号，范围为0-63.
/// \param ImmediateBatch       0:立即开始批处理  非0:等待外部开始批处理.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_StartMeasureWithCallback(int iDeviceId, int ImmediateBatch);

///
/// \brief SR7IF_TriggerOneBatch   批处理软件触发开始（获取数据模式:批处理一次回调一次）
/// \param lDeviceId            设备ID号，范围为0-63.
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_TriggerOneBatch(int iDeviceId);

///
/// \brief SR7IF_GetBatchProfilePoint   批处理轮廓获取（获取数据模式:批处理一次回调一次）
/// \param DataIndex            参数数据传递
/// \param Head                 0：相机头A  1：相机头B
/// \return
///     !=NULL:                 返回数据指针
///     =NULL:                  失败，无数据或者相应头不存在.
///
SR7_IF_API const int *SR7IF_GetBatchProfilePoint(const SR7IF_Data *DataIndex, int Head);

///
/// \brief SR7IF_GetBatchIntensityPoint   批处理亮度获取（获取数据模式:批处理一次回调一次）
/// \param DataIndex            参数数据传递
/// \param Head                 0：相机头A  1：相机头B
/// \return
///     !=NULL:                 返回数据指针
///     =NULL:                  失败，无数据或者相应头不存在.
///
SR7_IF_API const unsigned char *SR7IF_GetBatchIntensityPoint(const SR7IF_Data *DataIndex, int Head);

///
/// \brief SR7IF_GetBatchEncoderPoint   批处理编码器获取（获取数据模式:批处理一次回调一次）
/// \param DataIndex            参数数据传递
/// \param Head                 0：相机头A  1：相机头B
/// \return
///     !=NULL:                 返回数据指针
///     =NULL:                  失败，无数据或者相应头不存在.
///
SR7_IF_API const unsigned int *SR7IF_GetBatchEncoderPoint(const SR7IF_Data *DataIndex, int Head);

///
/// \brief SR7LinkSearchOnline   相机在线查询,EdgeImaging使用时、其他程序调用该接口或者被防火墙屏蔽时可能会搜索失败
/// \param ReadNum              搜索到在线相机数量
/// \param timeOut              搜索时间(ms),最小值500
/// \return
///     !=NULL:                 返回指向IP的指针
///     =NULL:                  失败.参数错误或者无在线相机
///
SR7_IF_API SR7IF_ETHERNET_CONFIG *SR7IF_SearchOnline(int *ReadNum, int timeOut);

///
/// \brief SR7IF_SetMultiEncoderInterval   设置多组编码器细分,EdgeImaging中设置的细分点数为起始批处理点
/// \param lDeviceId            设备ID号,范围为0-63.
/// \param DataObj              预留，设置为NULL
/// \param enable               0停止多组编码器细分，1：使能多组编码器细分
/// \param Point[1-8]           编码器作用的批处理起始点,范围[0-15000],0:该点不起效.要求写入数据成递增关系(如Point1 < Point2...)
/// \param Interval[1-8]        编码器对应的细分,范围[1-10000]
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_SetMultiEncoderInterval(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int enable,
                                  unsigned short Point1, unsigned short Interval1,
                                  unsigned short Point2, unsigned short Interval2,
                                  unsigned short Point3, unsigned short Interval3,
                                  unsigned short Point4, unsigned short Interval4,
                                  unsigned short Point5, unsigned short Interval5,
                                  unsigned short Point6, unsigned short Interval6,
                                  unsigned short Point7, unsigned short Interval7,
                                  unsigned short Point8, unsigned short Interval8);


///
/// \brief SR7IF_GetTimeStamp   本次上电后控制器运行的时间
/// \param lDeviceId            设备ID号,范围为0-63.
/// \param DataObj              预留,设置为NULL
/// \param TimeStamp            返回运行的时间(单位秒)
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetTimeStamp(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int *TimeStamp);

///
/// \brief SR7IF_GetStartIOTriggerCount   相机开始批处理IO被触发次数计数器，从上电开始计数
/// \param lDeviceId            设备ID号,范围为0-63.
/// \param DataObj              预留,设置为NULL
/// \param TriggerCount         返回触发次数
/// \return
///     <0:                     失败.
///     =0:                     成功.
///
SR7_IF_API int SR7IF_GetStartIOTriggerCount(unsigned int lDeviceId, const SR7IF_Data DataObj, unsigned int *pTriggerCount);

#ifdef __cplusplus
}
#endif
#endif //SR7LINK__H

