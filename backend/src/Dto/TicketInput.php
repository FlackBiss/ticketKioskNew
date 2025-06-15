<?php

namespace src\Dto;


class TicketInput
{
    public string $place;
    public float $price;
    public int $eventId;
    public string $type;
    public ?string $uuid;
    public string $email;
    public string $surname;
    public string $name;
    public int $count;
}