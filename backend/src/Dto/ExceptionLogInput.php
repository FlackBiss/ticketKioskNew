<?php

namespace src\Dto;

class ExceptionLogInput
{
    public int $terminalId;
    public ?string $name = '';
    public string $log;
    public string $code;
    public string $comment;
}