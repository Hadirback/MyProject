using System;

namespace EmailSender
{
    class Queries
    {
        public static string WRITE_RESPONCE_TO_LOG_NTF_TABLE =
            @"
BEGIN TRY
  BEGIN TRAN
  INSERT INTO ntf_service_log
    (
        recipient_id
        , notification_type_id
        , text
        , code
    )
    VALUES
    (
        @recipient_id
        , @notification_type_id
        , @text   
        , @code
    )

    IF @code = 1
        INSERT INTO ntf_notifications ([recipient_id], notification_type_id, [text], [sent], [date], [application_name])
        VALUES(@recipient_id, @notification_type_id, @text, 1, GETDATE(), @application_name)
    COMMIT TRAN
END TRY
BEGIN CATCH
  ROLLBACK TRAN
END CATCH
";

        public static String SELECT_CURRENT_SLOT_CLUB_NAME =
            @"
SELECT 
    [name] 
FROM slot_clubs 
WHERE club_id = dbo.get_active_slot_club_id()
";

        public static String SELECT_EVENT_CODE =
@"
SELECT code
FROM ntf_application_events
WHERE application_id = @application_id
	AND code_app = @code_app
";

        public const String SELECT_LICENSE_UID =
@"
SELECT value
FROM cas_adm_settings
WHERE name = 'slotservice_license_uid'
";

        public const String GET_RECIPIENTS_BY_EVENT_CODE =
@"
SELECT nr.id
       , nr.name
       , nr.phone_number
       , nr.email
       , nr.dns_name     
       , nr.language_id     
FROM ntf_recipients_notifications nrn
    INNER JOIN ntf_recipients nr ON nr.id = nrn.id_recipient
	INNER JOIN ntf_application_events nae ON nae.id = nrn.id_event
WHERE
    nae.code_app = @event_code
";

        public const String GET_EVENT_STRINGS =
@"
SELECT *
FROM ntf_application_events_localization
WHERE application_event_id IN 
(
	SELECT id
	FROM ntf_application_events
	WHERE application_id = @application_id
		AND code_app = @code_app
)
";

    }
}
