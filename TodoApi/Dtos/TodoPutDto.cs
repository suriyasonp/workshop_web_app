namespace TodoApi.Dtos;

public record TodoPutDto(
    string Title,
    bool IsCompleted
);