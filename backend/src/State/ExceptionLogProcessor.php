<?php

namespace src\State;

use ApiPlatform\Metadata\Operation;
use ApiPlatform\State\ProcessorInterface;
use src\Entity\ExceptionLog;
use src\Repository\ExceptionLogRepository;
use src\Repository\TerminalRepository;
use Symfony\Component\HttpFoundation\JsonResponse;
use function Symfony\Component\String\u;

readonly class ExceptionLogProcessor implements ProcessorInterface
{
    public function __construct(
        private TerminalRepository     $terminalRepository,
        private ExceptionLogRepository $exceptionLogRepository
    )
    {
    }

    /**
     * @throws \Exception
     */
    public function process(mixed $data, Operation $operation, array $uriVariables = [], array $context = []): ExceptionLog
    {
        if ($this->terminalRepository->find($data->terminalId) === null)
        {
            throw new \Exception('По введённому id, терминала не существует');
        }

        $exceptionLog = new ExceptionLog();
        $terminal = $this->terminalRepository->find($data->terminalId);

        $exceptionLog
            ->setName(u($data->name)->truncate(252, '...'))
            ->setTerminal($terminal)
            ->setLog($data->log)
            ->setCode($data->code)
            ->setComment($data->comment);

        $this->exceptionLogRepository->save($exceptionLog, true);

        return $exceptionLog;
    }
}