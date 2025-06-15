<?php

namespace src\Controller\Admin;

use src\Entity\Event;
use src\Entity\ExceptionLog;
use src\Entity\Information;
use App\Entity\Mail;
use src\Entity\News;
use src\Entity\Scheme;
use src\Entity\Place;
use src\Entity\Sessions;
use src\Entity\StandBy;
use src\Controller\Admin\EventCrudController;
use src\Entity\Terminal;
use src\Entity\Ticket;
use src\Entity\User;
use src\Repository\InformationRepository;
use EasyCorp\Bundle\EasyAdminBundle\Config\Action;
use EasyCorp\Bundle\EasyAdminBundle\Config\Dashboard;
use EasyCorp\Bundle\EasyAdminBundle\Config\MenuItem;
use EasyCorp\Bundle\EasyAdminBundle\Controller\AbstractDashboardController;
use EasyCorp\Bundle\EasyAdminBundle\Router\AdminUrlGenerator;
use Psr\Container\ContainerExceptionInterface;
use Psr\Container\NotFoundExceptionInterface;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Attribute\Route;

class DashboardController extends AbstractDashboardController
{
    public function __construct(
        private readonly InformationRepository $informationRepository,
    )
    {
    }

    /**
     * @throws NotFoundExceptionInterface
     * @throws ContainerExceptionInterface
     */
    #[Route(path: '/admin', name: 'admin')]
    public function index(): Response
    {
        $adminUrlGenerator = $this->container->get(AdminUrlGenerator::class);
        return $this->redirect($adminUrlGenerator->setController(EventCrudController::class)->generateUrl());
    }

    public function configureDashboard(): Dashboard
    {
        return Dashboard::new()
            ->setTitle('<span>Админ-панель</span>')
            ->setFaviconPath('favicon.ico')
            ->renderContentMaximized()
            ->generateRelativeUrls()
            ->disableDarkMode();
    }

    public function configureMenuItems(): iterable
    {
        yield MenuItem::section('Контент')
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Новости', 'fa fa-newspaper', News::class)
            ->setPermission('ROLE_ADMIN');
        if ($this->informationRepository->count([]) === 0) {
            yield MenuItem::linkToCrud('Информация', 'fa fa-info-circle', Information::class)
                ->setAction(Action::NEW)
                ->setPermission('ROLE_ADMIN');
        } else {
            yield MenuItem::linkToCrud('Информация', 'fa fa-info-circle', Information::class)
                ->setAction(Action::EDIT)->setEntityId($this->informationRepository->findAll()[0]->getId())
                ->setPermission('ROLE_ADMIN');
        }

        yield MenuItem::section('Мероприятия и билеты')
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Мероприятия', 'fa fa-calendar-check', Event::class)
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Билеты', 'fa fa-ticket-alt', Ticket::class)
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Схемы зала', 'fa fa-table', Scheme::class)
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Места', 'fa fa-chair', Place::class)
            ->setPermission('ROLE_ADMIN');

        yield MenuItem::section('Терминалы и сессии');
        yield MenuItem::linkToCrud('Терминалы', 'fa fa-desktop', Terminal::class)
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Сессии', 'fa fa-clock', Sessions::class);
        yield MenuItem::linkToCrud('Режим ожидания', 'fa fa-terminal', StandBy::class)
            ->setPermission('ROLE_ADMIN');

        yield MenuItem::section('Логи')
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Исключения', 'fa fa-bug', ExceptionLog::class)
            ->setPermission('ROLE_ADMIN');

        yield MenuItem::linkToRoute(
            'Статистика продаж',
            'fa fa-ticket-alt',
            'app_stats_ticket'
        );

        yield MenuItem::section('Прочее')
            ->setPermission('ROLE_ADMIN');
        yield MenuItem::linkToCrud('Пользователи', 'fas fa-user-gear', User::class)
            ->setPermission('ROLE_ADMIN');
    }
}
