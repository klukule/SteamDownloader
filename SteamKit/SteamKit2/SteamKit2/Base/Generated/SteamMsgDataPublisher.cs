// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: steammessages_datapublisher.steamclient.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace SteamKit2.Internal
{

    [global::ProtoBuf.ProtoContract()]
    public partial class CDataPublisher_ClientContentCorruptionReport_Notification : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint appid
        {
            get => __pbn__appid.GetValueOrDefault();
            set => __pbn__appid = value;
        }
        public bool ShouldSerializeappid() => __pbn__appid != null;
        public void Resetappid() => __pbn__appid = null;
        private uint? __pbn__appid;

        [global::ProtoBuf.ProtoMember(2)]
        public uint depotid
        {
            get => __pbn__depotid.GetValueOrDefault();
            set => __pbn__depotid = value;
        }
        public bool ShouldSerializedepotid() => __pbn__depotid != null;
        public void Resetdepotid() => __pbn__depotid = null;
        private uint? __pbn__depotid;

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string download_source
        {
            get => __pbn__download_source ?? "";
            set => __pbn__download_source = value;
        }
        public bool ShouldSerializedownload_source() => __pbn__download_source != null;
        public void Resetdownload_source() => __pbn__download_source = null;
        private string __pbn__download_source;

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue("")]
        public string objectid
        {
            get => __pbn__objectid ?? "";
            set => __pbn__objectid = value;
        }
        public bool ShouldSerializeobjectid() => __pbn__objectid != null;
        public void Resetobjectid() => __pbn__objectid = null;
        private string __pbn__objectid;

        [global::ProtoBuf.ProtoMember(5)]
        public uint cellid
        {
            get => __pbn__cellid.GetValueOrDefault();
            set => __pbn__cellid = value;
        }
        public bool ShouldSerializecellid() => __pbn__cellid != null;
        public void Resetcellid() => __pbn__cellid = null;
        private uint? __pbn__cellid;

        [global::ProtoBuf.ProtoMember(6)]
        public bool is_manifest
        {
            get => __pbn__is_manifest.GetValueOrDefault();
            set => __pbn__is_manifest = value;
        }
        public bool ShouldSerializeis_manifest() => __pbn__is_manifest != null;
        public void Resetis_manifest() => __pbn__is_manifest = null;
        private bool? __pbn__is_manifest;

        [global::ProtoBuf.ProtoMember(7)]
        public ulong object_size
        {
            get => __pbn__object_size.GetValueOrDefault();
            set => __pbn__object_size = value;
        }
        public bool ShouldSerializeobject_size() => __pbn__object_size != null;
        public void Resetobject_size() => __pbn__object_size = null;
        private ulong? __pbn__object_size;

        [global::ProtoBuf.ProtoMember(8)]
        public uint corruption_type
        {
            get => __pbn__corruption_type.GetValueOrDefault();
            set => __pbn__corruption_type = value;
        }
        public bool ShouldSerializecorruption_type() => __pbn__corruption_type != null;
        public void Resetcorruption_type() => __pbn__corruption_type = null;
        private uint? __pbn__corruption_type;

        [global::ProtoBuf.ProtoMember(9)]
        public bool used_https
        {
            get => __pbn__used_https.GetValueOrDefault();
            set => __pbn__used_https = value;
        }
        public bool ShouldSerializeused_https() => __pbn__used_https != null;
        public void Resetused_https() => __pbn__used_https = null;
        private bool? __pbn__used_https;

        [global::ProtoBuf.ProtoMember(10)]
        public bool oc_proxy_detected
        {
            get => __pbn__oc_proxy_detected.GetValueOrDefault();
            set => __pbn__oc_proxy_detected = value;
        }
        public bool ShouldSerializeoc_proxy_detected() => __pbn__oc_proxy_detected != null;
        public void Resetoc_proxy_detected() => __pbn__oc_proxy_detected = null;
        private bool? __pbn__oc_proxy_detected;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CDataPublisher_ClientUpdateAppJob_Notification : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint app_id
        {
            get => __pbn__app_id.GetValueOrDefault();
            set => __pbn__app_id = value;
        }
        public bool ShouldSerializeapp_id() => __pbn__app_id != null;
        public void Resetapp_id() => __pbn__app_id = null;
        private uint? __pbn__app_id;

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<uint> depot_ids { get; } = new global::System.Collections.Generic.List<uint>();

        [global::ProtoBuf.ProtoMember(3)]
        public uint app_state
        {
            get => __pbn__app_state.GetValueOrDefault();
            set => __pbn__app_state = value;
        }
        public bool ShouldSerializeapp_state() => __pbn__app_state != null;
        public void Resetapp_state() => __pbn__app_state = null;
        private uint? __pbn__app_state;

        [global::ProtoBuf.ProtoMember(4)]
        public uint job_app_error
        {
            get => __pbn__job_app_error.GetValueOrDefault();
            set => __pbn__job_app_error = value;
        }
        public bool ShouldSerializejob_app_error() => __pbn__job_app_error != null;
        public void Resetjob_app_error() => __pbn__job_app_error = null;
        private uint? __pbn__job_app_error;

        [global::ProtoBuf.ProtoMember(5)]
        [global::System.ComponentModel.DefaultValue("")]
        public string error_details
        {
            get => __pbn__error_details ?? "";
            set => __pbn__error_details = value;
        }
        public bool ShouldSerializeerror_details() => __pbn__error_details != null;
        public void Reseterror_details() => __pbn__error_details = null;
        private string __pbn__error_details;

        [global::ProtoBuf.ProtoMember(6)]
        public uint job_duration
        {
            get => __pbn__job_duration.GetValueOrDefault();
            set => __pbn__job_duration = value;
        }
        public bool ShouldSerializejob_duration() => __pbn__job_duration != null;
        public void Resetjob_duration() => __pbn__job_duration = null;
        private uint? __pbn__job_duration;

        [global::ProtoBuf.ProtoMember(7)]
        public uint files_validation_failed
        {
            get => __pbn__files_validation_failed.GetValueOrDefault();
            set => __pbn__files_validation_failed = value;
        }
        public bool ShouldSerializefiles_validation_failed() => __pbn__files_validation_failed != null;
        public void Resetfiles_validation_failed() => __pbn__files_validation_failed = null;
        private uint? __pbn__files_validation_failed;

        [global::ProtoBuf.ProtoMember(8)]
        public ulong job_bytes_downloaded
        {
            get => __pbn__job_bytes_downloaded.GetValueOrDefault();
            set => __pbn__job_bytes_downloaded = value;
        }
        public bool ShouldSerializejob_bytes_downloaded() => __pbn__job_bytes_downloaded != null;
        public void Resetjob_bytes_downloaded() => __pbn__job_bytes_downloaded = null;
        private ulong? __pbn__job_bytes_downloaded;

        [global::ProtoBuf.ProtoMember(9)]
        public ulong job_bytes_staged
        {
            get => __pbn__job_bytes_staged.GetValueOrDefault();
            set => __pbn__job_bytes_staged = value;
        }
        public bool ShouldSerializejob_bytes_staged() => __pbn__job_bytes_staged != null;
        public void Resetjob_bytes_staged() => __pbn__job_bytes_staged = null;
        private ulong? __pbn__job_bytes_staged;

        [global::ProtoBuf.ProtoMember(10)]
        public ulong bytes_comitted
        {
            get => __pbn__bytes_comitted.GetValueOrDefault();
            set => __pbn__bytes_comitted = value;
        }
        public bool ShouldSerializebytes_comitted() => __pbn__bytes_comitted != null;
        public void Resetbytes_comitted() => __pbn__bytes_comitted = null;
        private ulong? __pbn__bytes_comitted;

        [global::ProtoBuf.ProtoMember(11)]
        public uint start_app_state
        {
            get => __pbn__start_app_state.GetValueOrDefault();
            set => __pbn__start_app_state = value;
        }
        public bool ShouldSerializestart_app_state() => __pbn__start_app_state != null;
        public void Resetstart_app_state() => __pbn__start_app_state = null;
        private uint? __pbn__start_app_state;

        [global::ProtoBuf.ProtoMember(12, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong stats_machine_id
        {
            get => __pbn__stats_machine_id.GetValueOrDefault();
            set => __pbn__stats_machine_id = value;
        }
        public bool ShouldSerializestats_machine_id() => __pbn__stats_machine_id != null;
        public void Resetstats_machine_id() => __pbn__stats_machine_id = null;
        private ulong? __pbn__stats_machine_id;

        [global::ProtoBuf.ProtoMember(13)]
        [global::System.ComponentModel.DefaultValue("")]
        public string branch_name
        {
            get => __pbn__branch_name ?? "";
            set => __pbn__branch_name = value;
        }
        public bool ShouldSerializebranch_name() => __pbn__branch_name != null;
        public void Resetbranch_name() => __pbn__branch_name = null;
        private string __pbn__branch_name;

        [global::ProtoBuf.ProtoMember(14)]
        public ulong total_bytes_downloaded
        {
            get => __pbn__total_bytes_downloaded.GetValueOrDefault();
            set => __pbn__total_bytes_downloaded = value;
        }
        public bool ShouldSerializetotal_bytes_downloaded() => __pbn__total_bytes_downloaded != null;
        public void Resettotal_bytes_downloaded() => __pbn__total_bytes_downloaded = null;
        private ulong? __pbn__total_bytes_downloaded;

        [global::ProtoBuf.ProtoMember(15)]
        public ulong total_bytes_staged
        {
            get => __pbn__total_bytes_staged.GetValueOrDefault();
            set => __pbn__total_bytes_staged = value;
        }
        public bool ShouldSerializetotal_bytes_staged() => __pbn__total_bytes_staged != null;
        public void Resettotal_bytes_staged() => __pbn__total_bytes_staged = null;
        private ulong? __pbn__total_bytes_staged;

        [global::ProtoBuf.ProtoMember(16)]
        public ulong total_bytes_restored
        {
            get => __pbn__total_bytes_restored.GetValueOrDefault();
            set => __pbn__total_bytes_restored = value;
        }
        public bool ShouldSerializetotal_bytes_restored() => __pbn__total_bytes_restored != null;
        public void Resettotal_bytes_restored() => __pbn__total_bytes_restored = null;
        private ulong? __pbn__total_bytes_restored;

        [global::ProtoBuf.ProtoMember(17)]
        public bool is_borrowed
        {
            get => __pbn__is_borrowed.GetValueOrDefault();
            set => __pbn__is_borrowed = value;
        }
        public bool ShouldSerializeis_borrowed() => __pbn__is_borrowed != null;
        public void Resetis_borrowed() => __pbn__is_borrowed = null;
        private bool? __pbn__is_borrowed;

        [global::ProtoBuf.ProtoMember(18)]
        public bool is_free_weekend
        {
            get => __pbn__is_free_weekend.GetValueOrDefault();
            set => __pbn__is_free_weekend = value;
        }
        public bool ShouldSerializeis_free_weekend() => __pbn__is_free_weekend != null;
        public void Resetis_free_weekend() => __pbn__is_free_weekend = null;
        private bool? __pbn__is_free_weekend;

        [global::ProtoBuf.ProtoMember(20)]
        public ulong total_bytes_patched
        {
            get => __pbn__total_bytes_patched.GetValueOrDefault();
            set => __pbn__total_bytes_patched = value;
        }
        public bool ShouldSerializetotal_bytes_patched() => __pbn__total_bytes_patched != null;
        public void Resettotal_bytes_patched() => __pbn__total_bytes_patched = null;
        private ulong? __pbn__total_bytes_patched;

        [global::ProtoBuf.ProtoMember(21)]
        public ulong total_bytes_saved
        {
            get => __pbn__total_bytes_saved.GetValueOrDefault();
            set => __pbn__total_bytes_saved = value;
        }
        public bool ShouldSerializetotal_bytes_saved() => __pbn__total_bytes_saved != null;
        public void Resettotal_bytes_saved() => __pbn__total_bytes_saved = null;
        private ulong? __pbn__total_bytes_saved;

        [global::ProtoBuf.ProtoMember(22)]
        public uint cell_id
        {
            get => __pbn__cell_id.GetValueOrDefault();
            set => __pbn__cell_id = value;
        }
        public bool ShouldSerializecell_id() => __pbn__cell_id != null;
        public void Resetcell_id() => __pbn__cell_id = null;
        private uint? __pbn__cell_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CValveHWSurvey_GetSurveySchedule_Request : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string surveydatetoken
        {
            get => __pbn__surveydatetoken ?? "";
            set => __pbn__surveydatetoken = value;
        }
        public bool ShouldSerializesurveydatetoken() => __pbn__surveydatetoken != null;
        public void Resetsurveydatetoken() => __pbn__surveydatetoken = null;
        private string __pbn__surveydatetoken;

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong surveydatetokenversion
        {
            get => __pbn__surveydatetokenversion.GetValueOrDefault();
            set => __pbn__surveydatetokenversion = value;
        }
        public bool ShouldSerializesurveydatetokenversion() => __pbn__surveydatetokenversion != null;
        public void Resetsurveydatetokenversion() => __pbn__surveydatetokenversion = null;
        private ulong? __pbn__surveydatetokenversion;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CValveHWSurvey_GetSurveySchedule_Response : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint surveydatetoken
        {
            get => __pbn__surveydatetoken.GetValueOrDefault();
            set => __pbn__surveydatetoken = value;
        }
        public bool ShouldSerializesurveydatetoken() => __pbn__surveydatetoken != null;
        public void Resetsurveydatetoken() => __pbn__surveydatetoken = null;
        private uint? __pbn__surveydatetoken;

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong surveydatetokenversion
        {
            get => __pbn__surveydatetokenversion.GetValueOrDefault();
            set => __pbn__surveydatetokenversion = value;
        }
        public bool ShouldSerializesurveydatetokenversion() => __pbn__surveydatetokenversion != null;
        public void Resetsurveydatetokenversion() => __pbn__surveydatetokenversion = null;
        private ulong? __pbn__surveydatetokenversion;

    }

    public interface IDataPublisher
    {
        NoResponse ClientContentCorruptionReport(CDataPublisher_ClientContentCorruptionReport_Notification request);
        NoResponse ClientUpdateAppJobReport(CDataPublisher_ClientUpdateAppJob_Notification request);
    }

    public interface IValveHWSurvey
    {
        CValveHWSurvey_GetSurveySchedule_Response GetSurveySchedule(CValveHWSurvey_GetSurveySchedule_Request request);
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
