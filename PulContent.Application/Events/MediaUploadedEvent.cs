namespace PulContent.Application.Events;

public record MediaUploadedEvent(Guid JobId, Guid MediaAssetId);
