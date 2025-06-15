<?php

namespace src\Dto;

use src\Dto\AllEvent;

class SessionsInput
{
    public ?\DateTimeImmutable $startAt;
    public ?\DateTimeImmutable $endAt;
    public int $terminalId;

    /** @var AllEvent[] $allEvent */
    public array $allEvent;
}