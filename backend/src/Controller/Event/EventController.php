<?php

namespace src\Controller\Event;

use src\Repository\EventRepository;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use src\Entity\Event;

class EventController extends AbstractController
{
    public function __invoke(EventRepository $eventRepository): JsonResponse
    {
        $events = $eventRepository->findAll();

        usort($events, function (Event $a, Event $b) {
            return $a->getDateTimeAt() <=> $b->getDateTimeAt();
        });

        $dates = array_map(
            fn(Event $event) => $event->getDateTimeAt()->format('Y-m-d'),
            $events
        );

        $uniqueDates = array_values(array_unique($dates));

        return $this->json([
            'dates' => $uniqueDates,
        ]);
    }
}