using System.Buffers;
using SuperSocket.ProtoBase;

namespace SocketServer.PipelineFilter;

public class MyPipelineFilter : PipelineFilterBase<TextPackageInfo>
{
    public override TextPackageInfo Filter(ref SequenceReader<byte> reader)
    {
        // 获取输入流的所有剩余数据构成一个数据包
        var packet = reader.Sequence.Slice(0);

        try
        {
            // 将所有剩余数据包进行解码
            return DecodePackage(ref packet);
        }
        finally
        {
            // 将 reader 推进已处理数据的长度，即所有剩余数据的长度
            reader.Advance(packet.Length);
        }
    }
}